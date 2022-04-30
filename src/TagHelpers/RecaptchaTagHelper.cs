using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Microsoft.Extensions.Options;

namespace Griesoft.OrchardCore.ReCaptcha.TagHelpers
{
    /// <inheritdoc/>
    public class RecaptchaTagHelper : AspNetCore.ReCaptcha.TagHelpers.RecaptchaTagHelper
    {
        /// <inheritdoc/>
        public RecaptchaTagHelper(IOptionsMonitor<RecaptchaSettings> settings, IOptionsMonitor<RecaptchaOptions> options) 
            : base(settings, options)
        {
        }
    }
}
