using Griesoft.AspNetCore.ReCaptcha.TagHelpers;
using Griesoft.OrchardCore.ReCaptcha.Enums;
using OrchardCore.ContentManagement;

namespace Griesoft.OrchardCore.ReCaptcha.Models
{
    /// <summary>
    /// A ReCaptcha Invisible content part.
    /// </summary>
    public class RecaptchaInvisiblePart : ContentPart
    {
        /// <summary>
        /// The position of the reCAPTCHA badge.
        /// </summary>
        public BadgePosition Badge { get; set; }

        /// <summary>
        /// The tabindex of the challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.
        /// </summary>
        public int? TabIndex { get; set; }

        /// <summary>
        /// The Id of the form that this ReCaptcha is protecting.
        /// </summary>
        /// <remarks>
        /// If <see cref="Callback"/> is specified the value of this property will be ignored.
        /// If Callback is not specified a form id is required.
        /// </remarks>
        public string? FormId { get; set; }

        /// <summary>
        /// The name of your callback function, executed when the user submits a successful response. 
        /// The g-recaptcha-response token is passed to your callback.
        /// </summary>
        /// <remarks>
        /// If left empty a form Id is required and the part will render a script that will submit the form with the given Id.
        /// </remarks>
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

        /// <summary>
        /// The tag type that this element should be rendered as.
        /// </summary>
        public TagType TagType { get; set; }
    }
}
