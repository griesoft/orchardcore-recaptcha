using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Griesoft.OrchardCore.ReCaptcha.TagHelpers
{
    /// <inheritdoc/>
    /// <remarks>
    /// This tag helper only exists to override the namespace of the tag. To prevent name clashes by providing all
    /// tag helpers under the same namespace when importing them.
    /// </remarks>
    [HtmlTargetElement("recaptcha-invisible", Attributes = "callback", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("recaptcha-invisible", Attributes = "form-id", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("button", Attributes = "re-invisible,callback")]
    [HtmlTargetElement("button", Attributes = "re-invisible,form-id")]
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
