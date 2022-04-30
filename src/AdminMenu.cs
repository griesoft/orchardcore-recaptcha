using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.OrchardCore.ReCaptcha.Drivers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OrchardCore.Entities;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using System;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer S;

        public AdminMenu(IStringLocalizer<AdminMenu> stringLocalizer)
        {
            S = stringLocalizer;
        }

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
