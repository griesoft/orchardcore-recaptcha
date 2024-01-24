using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;

namespace Griesoft.OrchardCore.ReCaptcha.Workflows
{
    /// <summary>
    /// The activity driver for the <see cref="ValidateRecaptchaV3Task"/>.
    /// </summary>
    public class ValidateRecaptchaV3TaskDisplayDriver : ActivityDisplayDriver<ValidateRecaptchaV3Task, ValidateRecaptchaV3TaskViewModel>
    {
        /// <inheritdoc />
        protected override void EditActivity(ValidateRecaptchaV3Task source, ValidateRecaptchaV3TaskViewModel model)
        {
            model.Action = source.Action.Expression;
            model.ScoreThreshold = source.ScoreThreshold;
            model.FailOnBadScore = source.FailOnBadScore;
        }

        /// <inheritdoc />
        protected override void UpdateActivity(ValidateRecaptchaV3TaskViewModel model, ValidateRecaptchaV3Task activity)
        {
            activity.Action = new WorkflowExpression<string?>(model.Action);
            activity.ScoreThreshold = model.ScoreThreshold;
            activity.FailOnBadScore = model.FailOnBadScore;
        }
    }
}
