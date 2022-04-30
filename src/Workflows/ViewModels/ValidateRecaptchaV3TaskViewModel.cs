namespace Griesoft.OrchardCore.ReCaptcha.Workflows
{
    /// <summary>
    /// View model for <see cref="ValidateRecaptchaV3Task"/>.
    /// </summary>
    public class ValidateRecaptchaV3TaskViewModel
    {
        /// <summary>
        /// The reCAPTCHA V3 action name of the challenge.
        /// </summary>
        public string? Action { get; set; }
    }
}
