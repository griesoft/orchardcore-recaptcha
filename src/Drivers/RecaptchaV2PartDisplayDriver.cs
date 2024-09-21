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
    /// The ReCaptcha V2 content part display driver.
    /// </summary>
    public class RecaptchaV2PartDisplayDriver : ContentPartDisplayDriver<RecaptchaV2Part>
    {
        /// <inheritdoc />
        public override IDisplayResult Display(RecaptchaV2Part part, BuildPartDisplayContext context)
        {
            return Initialize<RecaptchaV2PartViewModel>(nameof(RecaptchaV2Part), model =>
            {
                model.Callback = part.Callback;
                model.Theme = part.Theme;
                model.ErrorCallback = part.ErrorCallback;
                model.ExpiredCallback = part.ExpiredCallback;
                model.TabIndex = part.TabIndex;
                model.Content = part.ContentText;
            })
            .Location("Content");
        }

        /// <inheritdoc />
        public override IDisplayResult Edit(RecaptchaV2Part part, BuildPartEditorContext context)
        {
            return Initialize<RecaptchaV2PartViewModel>($"{nameof(RecaptchaV2Part)}_Edit", model =>
            {
                model.Callback = part.Callback;
                model.Theme = part.Theme;
                model.ErrorCallback = part.ErrorCallback;
                model.ExpiredCallback = part.ExpiredCallback;
                model.TabIndex = part.TabIndex;
                model.Content = part.ContentText;
            });
        }

        /// <inheritdoc />
        public override async Task<IDisplayResult> UpdateAsync(RecaptchaV2Part part, UpdatePartEditorContext context)
        {
            _ = part ?? throw new ArgumentNullException(nameof(part));
            _ = context ?? throw new ArgumentNullException(nameof(context));

            var viewmodel = new RecaptchaV2PartViewModel();

            await context.Updater.TryUpdateModelAsync(viewmodel, Prefix);

            part.Callback = viewmodel.Callback;
            part.Theme = viewmodel.Theme;
            part.ErrorCallback = viewmodel.ErrorCallback;
            part.ExpiredCallback = viewmodel.ExpiredCallback;
            part.TabIndex = viewmodel.TabIndex;
            part.ContentText = viewmodel.Content;

            return Edit(part, context);
        }
    }
}
