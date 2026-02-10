using Fluid;
using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.OrchardCore.ReCaptcha.Drivers;
using Griesoft.OrchardCore.ReCaptcha.Models;
using Griesoft.OrchardCore.ReCaptcha.Services;
using Griesoft.OrchardCore.ReCaptcha.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;

namespace Griesoft.OrchardCore.ReCaptcha
{
    /// <inheritdoc />
    public class Startup : StartupBase
    {
        /// <inheritdoc />
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TemplateOptions>(options =>
            {
                options.MemberAccessStrategy.Register<RecaptchaV2PartViewModel>();
                options.MemberAccessStrategy.Register<RecaptchaInvisiblePartViewModel>();
                options.MemberAccessStrategy.Register<RecaptchaV3PartViewModel>();
            });

            services.AddRecaptchaService();

            services.AddTransient<IConfigureOptions<RecaptchaSettings>, RecaptchaSettingsConfiguration>();

            services.AddSiteDisplayDriver<RecaptchaSettingsDisplayDriver>();

            services.AddContentPart<RecaptchaV2Part>()
                .UseDisplayDriver<RecaptchaV2PartDisplayDriver>();

            services.AddContentPart<RecaptchaInvisiblePart>()
                .UseDisplayDriver<RecaptchaInvisiblePartDisplayDriver>();

            services.AddContentPart<RecaptchaV3Part>()
                .UseDisplayDriver<RecaptchaV3PartDisplayDriver>();

            services.AddScoped<IDataMigration, Migrations>();

            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<INavigationProvider, AdminMenu>();
        }
    }
}
