using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.AspNetCore.ReCaptcha.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha.Workflows.Activities
{
    /// <summary>
    /// A reCAPTCHA validation task base class.
    /// </summary>
    public abstract class ValidateRecaptchaTaskBase : TaskActivity
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IStringLocalizer S;

        private readonly IRecaptchaService _recaptchaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly RecaptchaOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="recaptchaService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="updateModelAccessor"></param>
        public ValidateRecaptchaTaskBase(IStringLocalizer<ValidateRecaptchaV2Task> localizer, IRecaptchaService recaptchaService,
            IHttpContextAccessor httpContextAccessor, IUpdateModelAccessor updateModelAccessor, IOptionsMonitor<RecaptchaOptions> options)
        {
            S = localizer;
            _recaptchaService = recaptchaService;
            _httpContextAccessor = httpContextAccessor;
            _updateModelAccessor = updateModelAccessor;
            _options = options.CurrentValue;
        }

        /// <inheritdoc />
        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(S["Done"], S["Valid"], S["Invalid"]);
        }

        /// <summary>
        /// Validate the <paramref name="context"/> contains a valid reCAPTCHA token.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected async Task<AspNetCore.ReCaptcha.ValidationResponse> ValidateToken(HttpContext context)
        {
            AspNetCore.ReCaptcha.ValidationResponse validationResponse;

            if (!TryGetRecaptchaToken(context.Request, out string? token))
            {
                validationResponse = new AspNetCore.ReCaptcha.ValidationResponse()
                {
                    Success = false
                };
            }
            else
            {
                validationResponse = await _recaptchaService.ValidateRecaptchaResponse(token, GetRemoteIp(_httpContextAccessor.HttpContext)).ConfigureAwait(true);
            }

            return validationResponse;
        }

        /// <summary>
        /// Try to add an error to the model state.
        /// </summary>
        /// <param name="error"></param>
        protected void TryAddModelError(string error)
        {
            if (_updateModelAccessor.ModelUpdater != null)
            {
                _updateModelAccessor.ModelUpdater.ModelState.TryAddModelError(RecaptchaServiceConstants.TokenKeyName, error);
            }
        }
        /// <summary>
        /// Get the remote IP address of the client.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string? GetRemoteIp(HttpContext? context)
        {
            return _options.UseRemoteIp ?
                context?.Connection.RemoteIpAddress?.ToString() :
                null;
        }
        /// <summary>
        /// Try to get the reCAPTCHA token out of the <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected static bool TryGetRecaptchaToken(HttpRequest request, [NotNullWhen(true)] out string? token)
        {
            if (request.Headers.ContainsKey(RecaptchaServiceConstants.TokenKeyName))
            {
                token = request.Headers[RecaptchaServiceConstants.TokenKeyName];
            }
            else if (request.HasFormContentType && request.Form.ContainsKey(RecaptchaServiceConstants.TokenKeyNameLower))
            {
                token = request.Form[RecaptchaServiceConstants.TokenKeyNameLower];
            }
            else if (request.Query.ContainsKey(RecaptchaServiceConstants.TokenKeyNameLower))
            {
                token = request.Query[RecaptchaServiceConstants.TokenKeyNameLower];
            }
            else
            {
                token = null;
            }

            return token != null;
        }
    }
}
