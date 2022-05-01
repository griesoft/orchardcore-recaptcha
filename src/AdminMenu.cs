using Griesoft.OrchardCore.ReCaptcha.Drivers;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha
{
    /// <summary>
    /// Recaptcha settings admin menu navigation provider.
    /// </summary>
    public class AdminMenu : INavigationProvider
    {
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
        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder.Add(S["Configuration"], configuration => configuration
                .Add(S["Settings"], settings => settings
                    .Add(S["ReCaptcha"], S["ReCaptcha"], demo => demo
                        .AddClass("recaptcha").Id("recaptcha")
                        .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = RecaptchaSettingsDisplayDriver.EditorGroupId })
                        .Permission(Permissions.ManageRecaptchaSettings)
                        .LocalNav()
                    )));

            return Task.CompletedTask;
        }
    }
}
