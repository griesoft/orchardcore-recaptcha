using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Griesoft.OrchardCore.ReCaptcha.TagHelpers
{
    /// <inheritdoc/>
    public class RecaptchaV3TagHelper : AspNetCore.ReCaptcha.TagHelpers.RecaptchaV3TagHelper
    {
        /// <inheritdoc/>
        public RecaptchaV3TagHelper(IOptionsMonitor<RecaptchaSettings> settings, ITagHelperComponentManager tagHelperComponentManager) 
            : base(settings, tagHelperComponentManager)
        {
        }
    }
}
