namespace Griesoft.OrchardCore.ReCaptcha.ViewModels
{
    /// <summary>
    /// View model for <see cref="AspNetCore.ReCaptcha.Configuration.RecaptchaSettings"/> 
    /// and <see cref="ReCaptcha.Drivers.RecaptchaSettingsDisplayDriver"/>.
    /// </summary>
    public class RecaptchaSettingsViewModel
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
    }
}
