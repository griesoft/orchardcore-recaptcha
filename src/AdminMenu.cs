using Griesoft.OrchardCore.ReCaptcha.Drivers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha
{
    /// <summary>
    /// Recaptcha settings admin menu navigation provider.
    /// </summary>
    public class AdminMenu : AdminNavigationProvider
    {
        private static readonly RouteValueDictionary _routeValues = new()
        {
            { "area", "OrchardCore.Settings" },
            { "groupId", RecaptchaSettingsDisplayDriver.EditorGroupId },
        };

        private readonly IStringLocalizer S;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringLocalizer"></param>
        public AdminMenu(IStringLocalizer<AdminMenu> stringLocalizer)
        {
            S = stringLocalizer;
        }

        /// <inheritdoc />
        protected override ValueTask BuildAsync(NavigationBuilder builder)
        {
            if (NavigationHelper.UseLegacyFormat())
            {
                builder.Add(S["Security"], security => security
                    .Add(S["Settings"], S["Settings"].PrefixPosition(), settings => settings
                        .Add(S["ReCaptcha"], S["ReCaptcha"].PrefixPosition(), recaptcha => recaptcha
                            .AddClass("recaptcha").Id("recaptcha")
                            .Action("Index", "Admin", _routeValues)
                            .Permission(Permissions.ManageRecaptchaSettings)
                            .LocalNav()
                        )));

                return ValueTask.CompletedTask;
            }

            builder.Add(S["Settings"], settings => settings
                .Add(S["Security"], S["Security"].PrefixPosition(), security => security
                    .Add(S["ReCaptcha"], S["ReCaptcha"].PrefixPosition(), recaptcha => recaptcha
                        .AddClass("recaptcha").Id("recaptcha")
                        .Action("Index", "Admin", _routeValues)
                        .Permission(Permissions.ManageRecaptchaSettings)
                        .LocalNav()
                    )));

            return ValueTask.CompletedTask;
        }
    }
}
