using Griesoft.AspNetCore.ReCaptcha.TagHelpers;
using OrchardCore.ContentManagement;

namespace Griesoft.OrchardCore.ReCaptcha.Models
{
    /// <summary>
    /// A ReCaptcha V2 content part.
    /// </summary>
    public class RecaptchaV2Part : ContentPart
    {
        /// <summary>
        /// The color theme of the widget.
        /// </summary>
        public Theme Theme { get; set; }

        /// <summary>
        /// The tabindex of the challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.
        /// </summary>
        public int? TabIndex { get; set; }

        /// <summary>
        /// Optional: The name of your callback function, executed when the user submits a successful response. 
        /// The g-recaptcha-response token is passed to your callback.
        /// </summary>
        public string? Callback { get; set; }

        /// <summary>
        /// The name of your callback function, executed when the reCAPTCHA response expires and the user needs to re-verify.
        /// </summary>
        public string? ExpiredCallback { get; set; }

        /// <summary>
        /// The name of your callback function, executed when reCAPTCHA encounters an error (usually network connectivity) 
        /// and cannot continue until connectivity is restored. If you specify a function here, you are responsible for 
        /// informing the user that they should retry.
        /// </summary>
        public string? ErrorCallback { get; set; }

        /// <summary>
        /// The content / text that will be rendered inside the submit button.
        /// </summary>
        public string? ContentText { get; set; } = string.Empty;
    }
}
