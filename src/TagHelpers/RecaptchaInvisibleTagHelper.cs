using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Griesoft.OrchardCore.ReCaptcha.TagHelpers
{
    /// <inheritdoc/>
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
