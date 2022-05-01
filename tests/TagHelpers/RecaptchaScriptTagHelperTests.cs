using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.OrchardCore.ReCaptcha.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Moq;
using OrchardCore.ResourceManagement;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace Griesoft.OrchardCore.ReCaptcha.Tests.TagHelpers
{
    public class RecaptchaScriptTagHelperTests
    {
        [Fact]
        public void Process_Should_RegisterFootScript()
        {
            var settingsMock = new Mock<IOptionsMonitor<RecaptchaSettings>>();
            var context = new TagHelperContext(new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));
            var output = new TagHelperOutput("recaptcha-script",
                new TagHelperAttributeList(), (useCachedResult, htmlEncoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent(string.Empty);
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            var resourceManagerMock = new Mock<IResourceManager>();
            resourceManagerMock
                .Setup(manager =>
                    manager.RegisterFootScript(It.Is<HtmlString>(val => 
                        val.Value != null && val.Value.StartsWith("<script"))))
                .Verifiable();
            var tagHelper = new RecaptchaScriptTagHelper(resourceManagerMock.Object, settingsMock.Object, HtmlEncoder.Default);

            tagHelper.Process(context, output);

            resourceManagerMock.Verify();
        }
    }
}
