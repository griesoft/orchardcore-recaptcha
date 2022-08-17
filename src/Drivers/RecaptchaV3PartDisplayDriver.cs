using Griesoft.OrchardCore.ReCaptcha.Models;
using Griesoft.OrchardCore.ReCaptcha.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha.Drivers
{
    /// <summary>
    /// The ReCaptcha V3 content part display driver.
    /// </summary>
    public class RecaptchaV3PartDisplayDriver : ContentPartDisplayDriver<RecaptchaV3Part>
    {
        /// <inheritdoc />
        public override IDisplayResult Display(RecaptchaV3Part part)
        {
            return Initialize<RecaptchaV3PartViewModel>(nameof(RecaptchaV3Part), model =>
            {
                model.FormId = part.FormId;
                model.Callback = part.Callback;
                model.Action = part.Action;
                model.Content = part.ContentText;
            })
            .Location("Content");
        }

        /// <inheritdoc />
        public override IDisplayResult Edit(RecaptchaV3Part part)
        {
            return Initialize<RecaptchaV3PartViewModel>($"{nameof(RecaptchaV3Part)}_Edit", model =>
            {
                model.FormId = part.FormId;
                model.Callback = part.Callback;
                model.Action = part.Action;
                model.Content = part.ContentText;
            });
        }

        /// <inheritdoc />
        public override async Task<IDisplayResult> UpdateAsync(RecaptchaV3Part part, IUpdateModel updater)
        {
            _ = part ?? throw new ArgumentNullException(nameof(part));
            _ = updater ?? throw new ArgumentNullException(nameof(updater));

            var viewmodel = new RecaptchaV3PartViewModel();

            await updater.TryUpdateModelAsync(viewmodel, Prefix);

            part.FormId = viewmodel.FormId;
            part.Callback = viewmodel.Callback;
            part.Action = viewmodel.Action;
            part.ContentText = viewmodel.Content;

            return Edit(part);
        }
    }
}
