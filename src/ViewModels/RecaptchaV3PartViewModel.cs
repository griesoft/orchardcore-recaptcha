using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Griesoft.OrchardCore.ReCaptcha.ViewModels
{
    /// <summary>
    /// The view model for <see cref="Models.RecaptchaV3Part"/>.
    /// </summary>
    public class RecaptchaV3PartViewModel : IValidatableObject
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
        /// <remarks>
        /// It is highly encouraged to make use of action names, that's why we require it. The action name is best to be unique
        /// for each form that is protected by ReCaptcha V3.
        /// </remarks>
        [Required]
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// The content / text that will be rendered inside the submit button.
        /// </summary>
        public string? Content { get; set; } = string.Empty;

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var localizer = validationContext.GetService<IStringLocalizer<RecaptchaInvisiblePartViewModel>>();

            if (string.IsNullOrWhiteSpace(FormId) && string.IsNullOrWhiteSpace(Callback))
            {
                var errorMessage = "Either specify the form id or the callback.";
                yield return new ValidationResult(localizer != null ? localizer[errorMessage] : errorMessage,
                    new[] { nameof(FormId), nameof(Callback) });
            }
        }
    }
}
