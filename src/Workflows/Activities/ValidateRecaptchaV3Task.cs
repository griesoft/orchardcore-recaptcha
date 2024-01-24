using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.AspNetCore.ReCaptcha.Services;
using Griesoft.OrchardCore.ReCaptcha.Workflows.Activities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha.Workflows
{
    /// <summary>
    /// A reCAPTCHA V3 challenge validation task for Orchard Core work flows.
    /// </summary>
    public class ValidateRecaptchaV3Task : ValidateRecaptchaTaskBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorkflowExpressionEvaluator _expressionEvaluator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="recaptchaService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="updateModelAccessor"></param>
        /// <param name="expressionEvaluator"></param>
        /// <param name="options"></param>
        public ValidateRecaptchaV3Task(IStringLocalizer<ValidateRecaptchaV2Task> localizer, IRecaptchaService recaptchaService,
            IHttpContextAccessor httpContextAccessor, IUpdateModelAccessor updateModelAccessor, IWorkflowExpressionEvaluator expressionEvaluator, 
            IOptionsMonitor<RecaptchaOptions> options)
            : base(localizer, recaptchaService, httpContextAccessor, updateModelAccessor, options)
        {
            _httpContextAccessor = httpContextAccessor;
            _expressionEvaluator = expressionEvaluator;
        }

        /// <inheritdoc />
        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(S["Done"], S["Valid"], S["Invalid"], S["Invalid Score"]);
        }

        /// <inheritdoc />
        public override string Name => nameof(ValidateRecaptchaV3Task);

        /// <inheritdoc />
        public override LocalizedString DisplayText => S["Validate reCAPTCHA V3 Task"];

        /// <inheritdoc />
        public override LocalizedString Category => S["Validation"];

        /// <summary>
        /// The reCAPTCHA V3 action name of the challenge.
        /// </summary>
        public WorkflowExpression<string?> Action
        {
            get => GetProperty(() => new WorkflowExpression<string?>());
            set => SetProperty(value);
        }

        /// <summary>
        /// The threshold that the score value should not fall below.
        /// </summary>
        public double ScoreThreshold
        {
            get => GetProperty(() => 0.8);
            set => SetProperty(value);
        }

        /// <summary>
        /// If true the task will return the 'Invalid' outcome, in case that the score falls below the <see cref="ScoreThreshold"/>.
        /// Otherwise the 'Valid' outcome will be returned in any case.
        /// </summary>
        public bool FailOnBadScore
        {
            get => GetProperty(() => false);
            set => SetProperty(value);
        }

        /// <inheritdoc />
        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return Outcomes("Invalid", "Done");
            }

            var response = await ValidateToken(_httpContextAccessor.HttpContext);

            if (!response.Success || response.Action != await _expressionEvaluator.EvaluateAsync(Action, workflowContext, null))
            {
                TryAddModelError("reCAPTCHA validation failed.");

                return Outcomes("Invalid", "Done");
            }

            // Add score to workflow properties
            if (response.Score != null)
            {
                workflowContext.Properties["RecaptchaScore"] = response.Score.Value;
            }

            if (response.Score == null || response.Score < ScoreThreshold)
            {
                TryAddModelError(S["reCAPTCHA score was below the threshold."]);
                return Outcomes(FailOnBadScore ? "Invalid" : "Valid", "Invalid Score", "Done");
            }

            return Outcomes("Valid", "Done");
        }
    }
}
