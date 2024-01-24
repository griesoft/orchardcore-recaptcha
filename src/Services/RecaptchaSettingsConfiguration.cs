using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Entities;
using OrchardCore.Settings;

namespace Griesoft.OrchardCore.ReCaptcha.Services
{
    /// <inheritdoc />
    public class RecaptchaSettingsConfiguration : IConfigureOptions<RecaptchaSettings>
    {
        private readonly ISiteService _siteService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteService"></param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="logger"></param>
        public RecaptchaSettingsConfiguration(ISiteService siteService, IDataProtectionProvider dataProtectionProvider,
            ILogger<RecaptchaSettingsConfiguration> logger)
        {
            _siteService = siteService;
            _dataProtectionProvider = dataProtectionProvider;
            _logger = logger;
        }

        /// <inheritdoc />
        public void Configure(RecaptchaSettings options)
        {
            var settings = _siteService.GetSiteSettingsAsync()
                .GetAwaiter().GetResult()
                .As<RecaptchaSettings>();

            if (string.IsNullOrEmpty(options.SiteKey))
            {
                options.SiteKey = settings.SiteKey;
            }

            if (string.IsNullOrEmpty(options.SecretKey))
            {
                try
                {
                    var protector = _dataProtectionProvider.CreateProtector(nameof(RecaptchaSettingsConfiguration));
                    options.SecretKey = protector.Unprotect(settings.SecretKey);
                }
                catch
                {
                    _logger.LogError("The SecretKey could not be decrypted. It may have been encrypted using a different key.");
                }
            }

            if (options.UseProxy == null)
            {
                options.UseProxy = settings.UseProxy;
            }

            if (string.IsNullOrEmpty(options.ProxyAddress))
            {
                options.ProxyAddress = settings.ProxyAddress;
            }

            // We can not know if this value was configured via app settings.
            // So we will always use the value specified in the settings.
            options.BypassOnLocal = settings.BypassOnLocal;
        }
    }
}
