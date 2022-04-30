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
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha.Drivers
{
    public class RecaptchaSettingsDisplayDriver : SectionDisplayDriver<ISite, RecaptchaSettings>
    {
        public const string EditorGroupId = "GriesoftRecaptcha";

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IShellHost _shellHost;
        private readonly ShellSettings _shellSettings;
        private readonly RecaptchaSettings _settings;

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

        public override async Task<IDisplayResult?> EditAsync(RecaptchaSettings section, BuildEditorContext context)
        {
            if (!await IsAuthorizedToManageRecaptchaSettingsAsync())
            {
                return null;
            }

            string secret = string.Empty;

            if (!string.IsNullOrWhiteSpace(section.SecretKey))
            {
                try
                {
                    var protector = _dataProtectionProvider.CreateProtector(nameof(RecaptchaSettingsConfiguration));
                    secret = protector.Unprotect(section.SecretKey);
                }
                catch { }
            }

            return Initialize<RecaptchaSettingsViewModel>($"{nameof(RecaptchaSettings)}_Edit", viewModel =>
            {
                viewModel.CanEditSiteKey = CanEditSiteKey(section);
                viewModel.CanEditSecretKey = CanEditSecretKey(secret);
                viewModel.SiteKey = section.SiteKey;
                viewModel.SecretKey = secret;
            })
            .Location("Content:1")
            .OnGroup(EditorGroupId);
        }
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
                else if (!string.IsNullOrWhiteSpace(viewModel.SecretKey) && CanEditSecretKey(viewModel.SecretKey))
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
        private bool CanEditSiteKey(RecaptchaSettings settings)
        {
            return _settings.SiteKey == settings.SiteKey;
        }
        private bool CanEditSecretKey(string secret)
        {
            return _settings.SecretKey == secret;
        }
    }
}
