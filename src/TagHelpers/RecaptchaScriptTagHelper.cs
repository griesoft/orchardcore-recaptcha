using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;
using System.IO;
using System.Text.Encodings.Web;

namespace Griesoft.OrchardCore.ReCaptcha.TagHelpers
{
    /// <inheritdoc/>
    public class RecaptchaScriptTagHelper : AspNetCore.ReCaptcha.TagHelpers.RecaptchaScriptTagHelper
    {
        private readonly IResourceManager _resourceManager;
        private readonly HtmlEncoder _htmlEncoder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceManager"></param>
        /// <param name="settings"></param>
        public RecaptchaScriptTagHelper(IResourceManager resourceManager, IOptionsMonitor<RecaptchaSettings> settings,
            HtmlEncoder htmlEncoder) : base(settings) 
        {
            _resourceManager = resourceManager;
            _htmlEncoder = htmlEncoder;
        }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            _resourceManager.RegisterFootScript(new HtmlString(RenderTagHelperOutput(output)));

            output.SuppressOutput();
        }

        private string RenderTagHelperOutput(TagHelperOutput output)
        {
            using var writer = new StringWriter();

            output.WriteTo(writer, _htmlEncoder);

            return writer.ToString();
        }
    }
}
