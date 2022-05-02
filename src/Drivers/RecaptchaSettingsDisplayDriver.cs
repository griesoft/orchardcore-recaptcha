using Griesoft.AspNetCore.ReCaptcha.Configuration;
using Griesoft.OrchardCore.ReCaptcha.Services;
using Griesoft.OrchardCore.ReCaptcha.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha.Drivers
{
    /// <summary>
    /// The display driver for the reCAPTCHA settings editor group.
    /// </summary>
    public class RecaptchaSettingsDisplayDriver : SectionDisplayDriver<ISite, RecaptchaSettings>
    {
        /// <summary>
        /// The settings editor group ID.
        /// </summary>
        public const string EditorGroupId = "GriesoftRecaptcha";

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IShellHost _shellHost;
        private readonly ShellSettings _shellSettings;
        private readonly RecaptchaSettings _settings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorizationService"></param>
        /// <param name="httpContext"></param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="shellHost"></param>
        /// <param name="shellSettings"></param>
        /// <param name="optionsMonitor"></param>
        public RecaptchaSettingsDisplayDriver(IAuthorizationService authorizationService, IHttpContextAccessor httpContext,
            IDataProtectionProvider dataProtectionProvider, IShellHost shellHost, ShellSettings shellSettings, 
            IOptionsMonitor<RecaptchaSettings> optionsMonitor)
        {
            _authorizationService = authorizationService;
            _httpContext = httpContext;
            _dataProtectionProvider = dataProtectionProvider;
            _shellHost = shellHost;
            _shellSettings = shellSettings;
            _settings = optionsMonitor.CurrentValue;
        }

        /// <inheritdoc />
        public override async Task<IDisplayResult?> EditAsync(RecaptchaSettings section, BuildEditorContext context)
        {
            if (!await IsAuthorizedToManageRecaptchaSettingsAsync())
            {
                return null;
            }

            return Initialize<RecaptchaSettingsViewModel>($"{nameof(RecaptchaSettings)}_Edit", viewModel =>
            {
                viewModel.CanEditSiteKey = CanEditSiteKey(section);
                viewModel.CanEditSecretKey = TryDecryptSecret(section.SecretKey, out var decrypted) && CanEditSecretKey(decrypted);
                viewModel.SiteKey = section.SiteKey;
                viewModel.SecretKey = decrypted;
            })
            .Location("Content:1")
            .OnGroup(EditorGroupId);
        }
        /// <inheritdoc />
        public override async Task<IDisplayResult?> UpdateAsync(RecaptchaSettings section, BuildEditorContext context)
        {
            if (context.GroupId == EditorGroupId)
            {
                if (!await IsAuthorizedToManageRecaptchaSettingsAsync())
                {
                    return null;
                }

                var viewModel = new RecaptchaSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

                if (CanEditSiteKey(section))
                {
                    section.SiteKey = viewModel.SiteKey ?? string.Empty;
                }

                // Reset the secret here
                if (string.IsNullOrWhiteSpace(viewModel.SecretKey) && !string.IsNullOrWhiteSpace(section.SecretKey))
                {
                    section.SecretKey = string.Empty;
                }
                // Only set the secret if not specified in appsettings.json
                else if (!string.IsNullOrWhiteSpace(viewModel.SecretKey) && 
                    TryDecryptSecret(section.SecretKey, out var decrypted) && CanEditSecretKey(decrypted))
                {
                    var protector = _dataProtectionProvider.CreateProtector(nameof(RecaptchaSettingsConfiguration));
                    section.SecretKey = protector.Protect(viewModel.SecretKey);
                }

                await _shellHost.ReleaseShellContextAsync(_shellSettings);
            }

            return await EditAsync(section, context);
        }

        private async Task<bool> IsAuthorizedToManageRecaptchaSettingsAsync()
        {
            var user = _httpContext.HttpContext?.User;

            return user != null && await _authorizationService.AuthorizeAsync(user, Permissions.ManageRecaptchaSettings);
        }
        private bool TryDecryptSecret(string encrypted, [NotNullWhen(true)] out string? decrypted)
        {
            decrypted = null;

            if (!string.IsNullOrWhiteSpace(encrypted))
            {
                try
                {
                    var protector = _dataProtectionProvider.CreateProtector(nameof(RecaptchaSettingsConfiguration));
                    decrypted = protector.Unprotect(encrypted);
                }
                catch 
                {
                    return false;
                }
            }
            else
            {
                decrypted = string.Empty;
            }

            return true;
        }
        private bool CanEditSiteKey(RecaptchaSettings settings)
        {
            return _settings.SiteKey == settings.SiteKey;
        }
        private bool CanEditSecretKey(string decryptedSecret)
        {
            return _settings.SecretKey == decryptedSecret;
        }
    }
}
