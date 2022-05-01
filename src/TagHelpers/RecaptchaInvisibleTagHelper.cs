using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Griesoft.OrchardCore.ReCaptcha.TagHelpers
{
    /// <inheritdoc/>
    /// <remarks>
    /// This tag helper only exists to override the namespace of the tag. To prevent name clashes by providing all
    /// tag helpers under the same namespace when importing them.
    /// </remarks>
    public class RecaptchaInvisibleTagHelper : AspNetCore.ReCaptcha.TagHelpers.RecaptchaInvisibleTagHelper
    {
        /// <inheritdoc/>
        public RecaptchaInvisibleTagHelper(IOptionsMonitor<RecaptchaSettings> settings, 
            IOptionsMonitor<RecaptchaOptions> options, ITagHelperComponentManager tagHelperComponentManager) 
            : base(settings, options, tagHelperComponentManager)
        {
        }
    }
}
