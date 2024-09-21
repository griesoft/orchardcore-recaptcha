using Griesoft.OrchardCore.ReCaptcha.Drivers;
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
            builder.Add(S["Configuration"], configuration => configuration
                .Add(S["Settings"], settings => settings
                    .Add(S["ReCaptcha"], S["ReCaptcha"], demo => demo
                        .AddClass("recaptcha").Id("recaptcha")
                        .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = RecaptchaSettingsDisplayDriver.EditorGroupId })
                        .Permission(Permissions.ManageRecaptchaSettings)
                        .LocalNav()
                    )));

            return ValueTask.CompletedTask;
        }
    }
}
