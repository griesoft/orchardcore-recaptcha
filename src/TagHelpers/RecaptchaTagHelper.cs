using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Microsoft.Extensions.Options;

namespace Griesoft.OrchardCore.ReCaptcha.TagHelpers
{
    /// <inheritdoc/>
    /// <remarks>
    /// This tag helper only exists to override the namespace of the tag. To prevent name clashes by providing all
    /// tag helpers under the same namespace when importing them.
    /// </remarks>
    public class RecaptchaTagHelper : AspNetCore.ReCaptcha.TagHelpers.RecaptchaTagHelper
    {
        /// <inheritdoc/>
        public RecaptchaTagHelper(IOptionsMonitor<RecaptchaSettings> settings, IOptionsMonitor<RecaptchaOptions> options) 
            : base(settings, options)
        {
        }
    }
}
