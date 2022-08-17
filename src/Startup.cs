using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.OrchardCore.ReCaptcha.Drivers;
using Griesoft.OrchardCore.ReCaptcha.Models;
using Griesoft.OrchardCore.ReCaptcha.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;

namespace Griesoft.OrchardCore.ReCaptcha
{
    /// <inheritdoc />
    public class Startup : StartupBase
    {
        private readonly IShellConfiguration _shellConfiguration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shellConfiguration"></param>
        public Startup(IShellConfiguration shellConfiguration)
        {
            _shellConfiguration = shellConfiguration;
        }

        /// <inheritdoc />
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddRecaptchaService();

            services.Configure<RecaptchaSettings>(_shellConfiguration.GetSection(RecaptchaServiceConstants.SettingsSectionKey));
            services.AddTransient<IConfigureOptions<RecaptchaSettings>, RecaptchaSettingsConfiguration>();
            services.AddScoped<IDisplayDriver<ISite>, RecaptchaSettingsDisplayDriver>();

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
