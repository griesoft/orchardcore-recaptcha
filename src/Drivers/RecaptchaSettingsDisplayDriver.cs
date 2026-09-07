using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.OrchardCore.ReCaptcha.Services;
using Griesoft.OrchardCore.ReCaptcha.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha.Drivers
{
    /// <summary>
    /// The display driver for the reCAPTCHA settings editor group.
    /// </summary>
    public class RecaptchaSettingsDisplayDriver : SiteDisplayDriver<RecaptchaSettings>
    {
        /// <summary>
        /// The settings editor group ID.
        /// </summary>
        public const string EditorGroupId = "GriesoftRecaptcha";

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IShellConfiguration _shellConfiguration;
        private readonly IShellReleaseManager _shellReleaseManager;

        /// <inheritdoc />
        protected override string SettingsGroupId => EditorGroupId;

        /// <summary>
        ///
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="httpContext"></param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="shellConfiguration"></param>
        /// <param name="shellReleaseManager"></param>
        public RecaptchaSettingsDisplayDriver(IAuthorizationService authorizationService, IHttpContextAccessor httpContext,
            IDataProtectionProvider dataProtectionProvider, IShellConfiguration shellConfiguration,
            IShellReleaseManager shellReleaseManager)
        {
            _authorizationService = authorizationService;
            _httpContext = httpContext;
            _dataProtectionProvider = dataProtectionProvider;
            _shellConfiguration = shellConfiguration;
            _shellReleaseManager = shellReleaseManager;
        }

        /// <inheritdoc />
        public override async Task<IDisplayResult?> EditAsync(ISite model, RecaptchaSettings section, BuildEditorContext context)
        {
            if (!await IsAuthorizedToManageRecaptchaSettingsAsync())
            {
                return null;
            }

            return Initialize<RecaptchaSettingsViewModel>($"{nameof(RecaptchaSettings)}_Edit", viewModel =>
            {
                viewModel.CanEditSiteKey = !IsConfiguredInShellConfiguration(nameof(RecaptchaSettings.SiteKey));
                viewModel.CanEditSecretKey = !IsConfiguredInShellConfiguration(nameof(RecaptchaSettings.SecretKey));
                viewModel.SiteKey = section.SiteKey;
                // The stored secret is write-only: it is kept encrypted in the site settings
                // and never rendered back into the editor.
                viewModel.SecretKey = string.Empty;
                viewModel.HasSecretKey = !string.IsNullOrWhiteSpace(section.SecretKey);
                viewModel.UseProxy = section.UseProxy ?? false;
                viewModel.ProxyAddress = section.ProxyAddress;
                viewModel.BypassOnLocal = section.BypassOnLocal;
            })
            .Location("Content:1")
            .OnGroup(SettingsGroupId);
        }
        /// <inheritdoc />
        public override async Task<IDisplayResult?> UpdateAsync(ISite model, RecaptchaSettings section, UpdateEditorContext context)
        {
            if (!await IsAuthorizedToManageRecaptchaSettingsAsync())
            {
                return null;
            }

            var viewModel = new RecaptchaSettingsViewModel();

            await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

            // Keys provided through the shell configuration (appsettings.json) always take
            // precedence, so editing the corresponding stored value is not allowed.
            if (!IsConfiguredInShellConfiguration(nameof(RecaptchaSettings.SiteKey)))
            {
                section.SiteKey = viewModel.SiteKey ?? string.Empty;
            }

            if (!IsConfiguredInShellConfiguration(nameof(RecaptchaSettings.SecretKey)))
            {
                if (viewModel.ClearSecretKey)
                {
                    section.SecretKey = string.Empty;
                }
                else if (!string.IsNullOrWhiteSpace(viewModel.SecretKey))
                {
                    // Replacing the secret must not depend on the old value being decryptable,
                    // so that a new secret can always be stored after a data protection
                    // key ring change.
                    var protector = _dataProtectionProvider.CreateProtector(nameof(RecaptchaSettingsConfiguration));
                    section.SecretKey = protector.Protect(viewModel.SecretKey);
                }
                // An empty input keeps the currently stored secret.
            }

            section.UseProxy = viewModel.UseProxy;
            section.ProxyAddress = viewModel.ProxyAddress;
            section.BypassOnLocal = viewModel.BypassOnLocal;

            _shellReleaseManager.RequestRelease();

            return await EditAsync(model, section, context);
        }

        private async Task<bool> IsAuthorizedToManageRecaptchaSettingsAsync()
        {
            var user = _httpContext.HttpContext?.User;

            return user != null && await _authorizationService.AuthorizeAsync(user, Permissions.ManageRecaptchaSettings);
        }
        private bool IsConfiguredInShellConfiguration(string settingName)
        {
            return !string.IsNullOrWhiteSpace(
                _shellConfiguration.GetSection(RecaptchaServiceConstants.SettingsSectionKey)[settingName]);
        }
    }
}
