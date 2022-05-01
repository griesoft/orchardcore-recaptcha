using Griesoft.OrchardCore.ReCaptcha;
using OrchardCore.Modules.Manifest;
using System.Reflection;

[assembly: Module(
    Name = "ReCaptcha",
    Author = "Griesinger Software",
    Website = "https://griesoft.com",
    Version = "0.8.0",
    Tags = new string[] { "security", "forms", "recaptchav2", "recaptchav3" }
)]

[assembly: Feature(
    Id = "Griesoft.OrchardCore.ReCaptcha",
    Name = "ReCaptcha",
    Description = "A Google reCAPTCHA module.",
    Category = "Security"
)] 