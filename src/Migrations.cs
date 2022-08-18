using Griesoft.OrchardCore.ReCaptcha.Models;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace Griesoft.OrchardCore.ReCaptcha
{
    /// <inheritdoc />
    public class Migrations : DataMigration
    {
        private readonly IStringLocalizer T;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentDefinitionManager"></param>
        /// <param name="stringLocalizer"></param>
        public Migrations(IStringLocalizer<Migrations> stringLocalizer, IContentDefinitionManager contentDefinitionManager)
        {
            T = stringLocalizer;
            _contentDefinitionManager = contentDefinitionManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(RecaptchaV2Part), builder => builder
                .WithDisplayName("ReCaptcha V2")
                .WithDescription(T["Adds a ReCaptcha V2 submit button element to your content item."]));

            _contentDefinitionManager.AlterTypeDefinition("RecaptchaV2", type => type
                .WithPart(nameof(RecaptchaV2Part))
                .Stereotype("Widget"));


            _contentDefinitionManager.AlterPartDefinition(nameof(RecaptchaInvisiblePart), builder => builder
                .WithDisplayName("ReCaptcha Invisible")
                .WithDescription(T["Adds a ReCaptcha invisible element to your content item."]));

            _contentDefinitionManager.AlterTypeDefinition("RecaptchaInvisible", type => type
                .WithPart(nameof(RecaptchaInvisiblePart))
                .Stereotype("Widget"));


            _contentDefinitionManager.AlterPartDefinition(nameof(RecaptchaV3Part), builder => builder
                .WithDisplayName("ReCaptcha V3")
                .WithDescription(T["Adds a ReCaptcha V3 submit button element to your content item."]));

            _contentDefinitionManager.AlterTypeDefinition("RecaptchaV3", type => type
                .WithPart(nameof(RecaptchaV3Part))
                .Stereotype("Widget"));

            return 1;
        }
    }
}
