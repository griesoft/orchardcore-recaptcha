using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.AspNetCore.ReCaptcha.Services;
using Griesoft.OrchardCore.ReCaptcha.Workflows.Activities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha.Workflows
{
    /// <summary>
    /// A reCAPTCHA V2 challenge validation task for Orchard Core work flows.
    /// </summary>
    public class ValidateRecaptchaV2Task : ValidateRecaptchaTaskBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="recaptchaService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="updateModelAccessor"></param>
        /// <param name="options"></param>
        public ValidateRecaptchaV2Task(IStringLocalizer<ValidateRecaptchaV2Task> localizer, IRecaptchaService recaptchaService,
            IHttpContextAccessor httpContextAccessor, IUpdateModelAccessor updateModelAccessor, IOptionsMonitor<RecaptchaOptions> options)
            : base(localizer, recaptchaService, httpContextAccessor, updateModelAccessor, options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public override string Name => nameof(ValidateRecaptchaV2Task);

        /// <inheritdoc />
        public override LocalizedString DisplayText => S["Validate reCAPTCHA V2 Task"];

        /// <inheritdoc />
        public override LocalizedString Category => S["Validation"];

        /// <inheritdoc />
        public override bool HasEditor => false;

        /// <inheritdoc />
        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return Outcomes("Invalid", "Done");
            }

            var response = await ValidateToken(_httpContextAccessor.HttpContext);

            if (!response.Success)
            {
                TryAddModelError("reCAPTCHA validation failed.");

                return Outcomes("Invalid", "Done");
            }

            return Outcomes("Valid", "Done");
        }
    }
}
