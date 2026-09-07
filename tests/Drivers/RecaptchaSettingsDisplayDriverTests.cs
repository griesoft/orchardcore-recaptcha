using Griesoft.OrchardCore.ReCaptcha.Drivers;
using Griesoft.OrchardCore.ReCaptcha.Services;
using Microsoft.AspNetCore.DataProtection;
using Xunit;

namespace Griesoft.OrchardCore.ReCaptcha.Tests.Drivers
{
    public class RecaptchaSettingsDisplayDriverTests
    {
        private readonly IDataProtectionProvider _dataProtectionProvider = new EphemeralDataProtectionProvider();

        [Fact]
        public void CanDecryptSecret_WithMatchingKeyRing_ReturnsTrue()
        {
            var protectedSecret = Protect("secret-value", _dataProtectionProvider);

            Assert.True(RecaptchaSettingsDisplayDriver.CanDecryptSecret(protectedSecret, _dataProtectionProvider));
        }

        [Fact]
        public void CanDecryptSecret_WithDifferentKeyRing_ReturnsFalse()
        {
            // Each ephemeral provider has its own key ring, which simulates the stored secret
            // having been protected on a host whose keys are gone.
            var protectedSecret = Protect("secret-value", new EphemeralDataProtectionProvider());

            Assert.False(RecaptchaSettingsDisplayDriver.CanDecryptSecret(protectedSecret, _dataProtectionProvider));
        }

        [Fact]
        public void CanDecryptSecret_WithUnprotectedValue_ReturnsFalse()
        {
            Assert.False(RecaptchaSettingsDisplayDriver.CanDecryptSecret("plaintext-not-ciphertext", _dataProtectionProvider));
        }

        [Fact]
        public void GetUpdatedSecretKey_WithNewSecret_StoresProtectedTrimmedValue()
        {
            var result = RecaptchaSettingsDisplayDriver.GetUpdatedSecretKey(" new-secret \n", clearRequested: false,
                currentSecretKey: string.Empty, _dataProtectionProvider);

            Assert.NotEqual("new-secret", result);
            Assert.Equal("new-secret", Unprotect(result));
        }

        [Fact]
        public void GetUpdatedSecretKey_WithNewSecretAndClearRequested_StoresNewSecret()
        {
            var currentSecret = Protect("old-secret", _dataProtectionProvider);

            var result = RecaptchaSettingsDisplayDriver.GetUpdatedSecretKey("new-secret", clearRequested: true,
                currentSecret, _dataProtectionProvider);

            Assert.Equal("new-secret", Unprotect(result));
        }

        [Fact]
        public void GetUpdatedSecretKey_WithNewSecret_WhenCurrentSecretUnreadable_StoresNewSecret()
        {
            var unreadableSecret = Protect("old-secret", new EphemeralDataProtectionProvider());

            var result = RecaptchaSettingsDisplayDriver.GetUpdatedSecretKey("new-secret", clearRequested: false,
                unreadableSecret, _dataProtectionProvider);

            Assert.Equal("new-secret", Unprotect(result));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void GetUpdatedSecretKey_WithEmptyInput_KeepsCurrentSecret(string? submittedSecretKey)
        {
            var currentSecret = Protect("old-secret", _dataProtectionProvider);

            var result = RecaptchaSettingsDisplayDriver.GetUpdatedSecretKey(submittedSecretKey, clearRequested: false,
                currentSecret, _dataProtectionProvider);

            Assert.Equal(currentSecret, result);
        }

        [Fact]
        public void GetUpdatedSecretKey_WithClearRequestedAndEmptyInput_ClearsSecret()
        {
            var currentSecret = Protect("old-secret", _dataProtectionProvider);

            var result = RecaptchaSettingsDisplayDriver.GetUpdatedSecretKey(string.Empty, clearRequested: true,
                currentSecret, _dataProtectionProvider);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetUpdatedSecretKey_WithClearRequestedAndUnreadableSecret_ClearsSecret()
        {
            var unreadableSecret = Protect("old-secret", new EphemeralDataProtectionProvider());

            var result = RecaptchaSettingsDisplayDriver.GetUpdatedSecretKey(null, clearRequested: true,
                unreadableSecret, _dataProtectionProvider);

            Assert.Equal(string.Empty, result);
        }

        private static string Protect(string value, IDataProtectionProvider provider)
        {
            return provider.CreateProtector(nameof(RecaptchaSettingsConfiguration)).Protect(value);
        }

        private string Unprotect(string protectedValue)
        {
            return _dataProtectionProvider.CreateProtector(nameof(RecaptchaSettingsConfiguration)).Unprotect(protectedValue);
        }
    }
}
