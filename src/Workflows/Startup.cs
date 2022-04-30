using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;

namespace Griesoft.OrchardCore.ReCaptcha.Workflows
{
    [RequireFeatures("OrchardCore.Workflows")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddActivity<ValidateRecaptchaV2Task, ValidateRecaptchaV2TaskDisplayDriver>();
            services.AddActivity<ValidateRecaptchaV3Task, ValidateRecaptchaV3TaskDisplayDriver>();
        }
    }
}
