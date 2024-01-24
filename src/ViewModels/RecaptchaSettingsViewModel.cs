using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Griesoft.OrchardCore.ReCaptcha.ViewModels
{
    /// <summary>
    /// View model for <see cref="AspNetCore.ReCaptcha.Configuration.RecaptchaSettings"/> 
    /// and <see cref="ReCaptcha.Drivers.RecaptchaSettingsDisplayDriver"/>.
    /// </summary>
    public class RecaptchaSettingsViewModel : IValidatableObject
    {
        /// <summary>
        /// The reCAPTCHA site key.
        /// </summary>
        public string? SiteKey { get; set; } = string.Empty;

        /// <summary>
        /// The reCAPTCHA secret key.
        /// </summary>
        public string? SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the site key can be modified or not.
        /// </summary>
        public bool CanEditSiteKey { get; set; }

        /// <summary>
        /// Indicates whether the secret key can be modified or not.
        /// </summary>
        public bool CanEditSecretKey { get; set; }

        /// <summary>
        /// Whether to use a proxy server or not for reCaptcha validation.
        /// </summary>
        public bool UseProxy { get; set; }

        /// <summary>
        /// Proxy server address to be used on the HTTP client.
        /// </summary>
        public string? ProxyAddress { get; set; }

        /// <summary>
        /// Indicates whether to bypass proxy for local addresses.
        /// </summary>
        public bool BypassOnLocal { get; set; }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var localizer = validationContext.GetService<IStringLocalizer<RecaptchaSettingsViewModel>>();

            if (UseProxy && string.IsNullOrWhiteSpace(ProxyAddress))
            {
                var errorMessage = "The proxy address must be specified if 'Use Proxy' is checked.";
                yield return new ValidationResult(localizer != null ? localizer[errorMessage] : errorMessage,
                    new[] { nameof(ProxyAddress) });
            }
        }
    }
}
