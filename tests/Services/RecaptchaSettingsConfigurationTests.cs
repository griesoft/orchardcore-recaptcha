using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.OrchardCore.ReCaptcha.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using OrchardCore.Settings;
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
            var rsc = new RecaptchaSettingsConfiguration(siteServiceMock, null!, null!);

            var appsettings = new RecaptchaSettings() { SiteKey = "test", SecretKey = "test" };
            rsc.Configure(appsettings);

            Assert.Equal("test", appsettings.SiteKey);
            Assert.Equal("test", appsettings.SecretKey);
        }

        [Fact]
        public void Configure_ShouldSet_SiteKey_FromSettings()
        {
            var siteServiceMock = CreateSiteServiceMock("not_test", "not_test");
            var rsc = new RecaptchaSettingsConfiguration(siteServiceMock, null!, null!);

            var appsettings = new RecaptchaSettings() { SiteKey = string.Empty, SecretKey = "test" };
            rsc.Configure(appsettings);

            Assert.Equal("not_test", appsettings.SiteKey);
        }

        [Fact]
        public void Configure_ShouldDecryptAndSet_SecretKey_FromSettings()
        {
            var siteServiceMock = CreateSiteServiceMock("not_test", "not_test_encrypted");
            var (decrypterMock, dataProtecterMock) = CreateDataProtectionProviderMock("not_test_encrypted", "not_test_decrypted");
            var rsc = new RecaptchaSettingsConfiguration(siteServiceMock, decrypterMock.Object, null!);

            var appsettings = new RecaptchaSettings() { SiteKey = string.Empty, SecretKey = string.Empty };
            rsc.Configure(appsettings);

            dataProtecterMock.Verify();
            Assert.Equal("not_test_decrypted", appsettings.SecretKey);
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
