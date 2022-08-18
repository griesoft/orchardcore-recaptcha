using OrchardCore.ContentManagement;

namespace Griesoft.OrchardCore.ReCaptcha.Models
{
    /// <summary>
    /// A ReCaptcha V3 content part.
    /// </summary>
    public class RecaptchaV3Part : ContentPart
    {
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
        /// An action name that will be used on server side ReCaptcha token validation. 
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// The content / text that will be rendered inside the submit button.
        /// </summary>
        public string? ContentText { get; set; } = string.Empty;
    }
}
