using OrchardCore.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageRecaptchaSettings = new(
            nameof(ManageRecaptchaSettings),
            "Manage reCAPTCHA settings.");

        public Task<IEnumerable<Permission>> GetPermissionsAsync() => Task.FromResult(new[]
        {
            ManageRecaptchaSettings,
        }
        .AsEnumerable());

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
        new[]
        {
            new PermissionStereotype
            {
                Name = "Administrator",
                Permissions = new[] { ManageRecaptchaSettings },
            },
        };
    }
}
