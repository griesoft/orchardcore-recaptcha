using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// The threshold that the score value should not fall below.
        /// </summary>
        /// <remarks>Must be in the range of 0-1. The default value is 0.8.</remarks>
        [Range(0.0, 1.0), DefaultValue(0.8)]
        public double ScoreThreshold { get; set; }

        /// <summary>
        /// If true the task will return the 'Invalid' outcome, in case that the score falls below the <see cref="ScoreThreshold"/>.
        /// Otherwise the 'Valid' outcome will be returned in any case.
        /// </summary>
        public bool FailOnBadScore { get; set; }
    }
}
