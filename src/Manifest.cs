using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "ReCaptcha",
    Author = "Griesinger Software",
    Website = "https://griesoft.com",
    Version = "1.0.0",
    Tags = new string[] { "security", "validation", "blockbots" }
)]

[assembly: Feature(
    Id = "Griesoft.OrchardCore.ReCaptcha",
    Name = "ReCaptcha",
    Description = "A Google reCAPTCHA module.",
    Category = "Security"
)] 