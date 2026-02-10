using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.OrchardCore.ReCaptcha.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Moq;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Settings;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Griesoft.OrchardCore.ReCaptcha.Tests.Services
{
    public class RecaptchaSettingsConfigurationTests
    {
        [Fact]
        public void Configure_ShouldNotOverride_SiteAndSecretKey()
        {
            var siteServiceMock = CreateSiteServiceMock("not_test", "not_test");
            var shellConfig = CreateShellConfiguration(siteKey: "test", secretKey: "test");
            var rsc = new RecaptchaSettingsConfiguration(siteServiceMock, shellConfig, null!, null!);

            var options = new RecaptchaSettings();
            rsc.Configure(options);

            Assert.Equal("test", options.SiteKey);
            Assert.Equal("test", options.SecretKey);
        }

        [Fact]
        public void Configure_ShouldSet_SiteKey_FromSettings()
        {
            var siteServiceMock = CreateSiteServiceMock("not_test", "not_test");
            var shellConfig = CreateShellConfiguration(secretKey: "test");
            var rsc = new RecaptchaSettingsConfiguration(siteServiceMock, shellConfig, null!, null!);

            var options = new RecaptchaSettings();
            rsc.Configure(options);

            Assert.Equal("not_test", options.SiteKey);
        }

        [Fact]
        public void Configure_ShouldDecryptAndSet_SecretKey_FromSettings()
        {
            var siteServiceMock = CreateSiteServiceMock("not_test", "AQID");
            var shellConfig = CreateShellConfiguration();
            var (decrypterMock, dataProtecterMock) = CreateDataProtectionProviderMock("AQID", "not_test_decrypted");
            var rsc = new RecaptchaSettingsConfiguration(siteServiceMock, shellConfig, decrypterMock.Object, null!);

            var options = new RecaptchaSettings();
            rsc.Configure(options);

            dataProtecterMock.Verify();
            Assert.Equal("not_test_decrypted", options.SecretKey);
        }

        private IShellConfiguration CreateShellConfiguration(string? siteKey = null, string? secretKey = null)
        {
            var dict = new Dictionary<string, string?>();
            var sectionKey = RecaptchaServiceConstants.SettingsSectionKey;
            if (siteKey != null) dict[$"{sectionKey}:{nameof(RecaptchaSettings.SiteKey)}"] = siteKey;
            if (secretKey != null) dict[$"{sectionKey}:{nameof(RecaptchaSettings.SecretKey)}"] = secretKey;

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dict)
                .Build();

            var shellConfigMock = new Mock<IShellConfiguration>();
            shellConfigMock.Setup(c => c.GetSection(sectionKey))
                .Returns(config.GetSection(sectionKey));
            return shellConfigMock.Object;
        }

        private ISiteService CreateSiteServiceMock(string settingsSiteKey, string settingsSecretKey)
        {
            var siteMock = new Mock<ISite>();
            siteMock.Setup(site => site.As<RecaptchaSettings>())
                .Returns(new RecaptchaSettings() { SiteKey = settingsSiteKey, SecretKey = settingsSecretKey });
            var siteServiceMock = new Mock<ISiteService>();
            siteServiceMock.Setup(service => service.GetSiteSettingsAsync())
                .Returns(Task.FromResult(siteMock.Object));

            return siteServiceMock.Object;
        }

        private (Mock<IDataProtectionProvider>, Mock<IDataProtector>) CreateDataProtectionProviderMock(string secretKeyEncrypted, string secretKeyDecrypted)
        {
            var dataProtectorMock = new Mock<IDataProtector>();
            dataProtectorMock.Setup(service => service.Unprotect(WebEncoders.Base64UrlDecode(secretKeyEncrypted)))
                .Returns(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetBytes(secretKeyDecrypted))
                .Verifiable();
            var decrypterMock = new Mock<IDataProtectionProvider>();
            decrypterMock.Setup(service => service.CreateProtector(It.Is<string>(val => val == nameof(RecaptchaSettingsConfiguration))))
                .Returns(dataProtectorMock.Object);

            return (decrypterMock, dataProtectorMock);
        }
    }
}
