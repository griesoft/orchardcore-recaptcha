using Griesoft.OrchardCore.ReCaptcha.Models;
using Griesoft.OrchardCore.ReCaptcha.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha.Drivers
{
    /// <summary>
    /// The ReCaptcha invisible content part display driver.
    /// </summary>
    public class RecaptchaInvisiblePartDisplayDriver : ContentPartDisplayDriver<RecaptchaInvisiblePart>
    {
        /// <inheritdoc />
        public override IDisplayResult Display(RecaptchaInvisiblePart part, BuildPartDisplayContext context)
        {
            return Initialize<RecaptchaInvisiblePartViewModel>(nameof(RecaptchaInvisiblePart), model =>
            {
                model.FormId = part.FormId;
                model.Callback = part.Callback;
                model.Badge = part.Badge;
                model.ErrorCallback = part.ErrorCallback;
                model.ExpiredCallback = part.ExpiredCallback;
                model.TabIndex = part.TabIndex;
                model.Content = part.ContentText;
                model.TagType = part.TagType;
            })
            .Location("Content");
        }

        /// <inheritdoc />
        public override IDisplayResult Edit(RecaptchaInvisiblePart part, BuildPartEditorContext context)
        {
            return Initialize<RecaptchaInvisiblePartViewModel>($"{nameof(RecaptchaInvisiblePart)}_Edit", model =>
            {
                model.FormId = part.FormId;
                model.Callback = part.Callback;
                model.Badge = part.Badge;
                model.ErrorCallback = part.ErrorCallback;
                model.ExpiredCallback = part.ExpiredCallback;
                model.TabIndex = part.TabIndex;
                model.Content = part.ContentText;
                model.TagType = part.TagType;
            });
        }

        /// <inheritdoc />
        public override async Task<IDisplayResult> UpdateAsync(RecaptchaInvisiblePart part, UpdatePartEditorContext context)
        {
            _ = part ?? throw new ArgumentNullException(nameof(part));
            _ = context ?? throw new ArgumentNullException(nameof(context));

            var viewmodel = new RecaptchaInvisiblePartViewModel();

            await context.Updater.TryUpdateModelAsync(viewmodel, Prefix);

            part.FormId = viewmodel.FormId;
            part.Callback = viewmodel.Callback;
            part.Badge = viewmodel.Badge;
            part.ErrorCallback = viewmodel.ErrorCallback;
            part.ExpiredCallback = viewmodel.ExpiredCallback;
            part.TabIndex = viewmodel.TabIndex;
            part.ContentText = viewmodel.Content;
            part.TagType = viewmodel.TagType;

            return Edit(part);
        }
    }
}
