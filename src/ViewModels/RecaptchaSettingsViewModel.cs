namespace Griesoft.OrchardCore.ReCaptcha.ViewModels
{
    public class RecaptchaSettingsViewModel
    {
        public string? SiteKey { get; set; } = string.Empty;

        public string? SecretKey { get; set; } = string.Empty;

        public bool CanEditSiteKey { get; set; }

        public bool CanEditSecretKey { get; set; }
    }
}
