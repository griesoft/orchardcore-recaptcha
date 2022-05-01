using OrchardCore.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Griesoft.OrchardCore.ReCaptcha
{
    /// <summary>
    /// Recaptcha settings permissions.
    /// </summary>
    public class Permissions : IPermissionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Permission ManageRecaptchaSettings = new(
            nameof(ManageRecaptchaSettings),
            "Manage reCAPTCHA settings.");

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Permission>> GetPermissionsAsync() => Task.FromResult(new[]
        {
            ManageRecaptchaSettings,
        }
        .AsEnumerable());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
