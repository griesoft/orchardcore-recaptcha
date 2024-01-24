using OrchardCore.Modules.Manifest;
using System.Reflection;

[assembly: Module(
    Name = "ReCaptcha",
    Author = "Griesoft",
    Website = "https://griesoft.com",
    Version = "1.0.0",
    Tags = new string[] { "security", "forms", "recaptchav2", "recaptchav3" }
)]

[assembly: Feature(
    Id = "Griesoft.OrchardCore.ReCaptcha",
    Name = "ReCaptcha",
    Description = "A Google reCAPTCHA module provided to you by the Griesoft team.",
    Category = "Security"
)]