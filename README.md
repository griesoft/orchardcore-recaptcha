# Orchard Core reCAPTCHA Module
An Orchard Core reCAPTCHA module that is based on our [ASP.NET Core reCAPTCHA](https://github.com/griesoft/aspnetcore-recaptcha) service.

[![Build Status](https://dev.azure.com/griesingersoftware/Orchard%20Core%20reCAPTCHA%20Module/_apis/build/status/CI%20Pipeline?branchName=main)](https://dev.azure.com/griesingersoftware/Orchard%20Core%20reCAPTCHA%20Module/_apis/build/status/CI%20Pipeline?branchName=main)
[![Build Status](https://vsrm.dev.azure.com/griesingersoftware/_apis/public/Release/badge/a7959783-e730-4a16-8ec8-436620f88501/1/2)](https://vsrm.dev.azure.com/griesingersoftware/_apis/public/Release/badge/a7959783-e730-4a16-8ec8-436620f88501/1/2)
[![License](https://badgen.net/github/license/griesoft/orchardcore-recaptcha)](https://github.com/griesoft/orchardcore-recaptcha/blob/master/LICENSE)
[![NuGet](https://badgen.net/nuget/v/Griesoft.OrchardCore.ReCaptcha)](https://www.nuget.org/packages/Griesoft.OrchardCore.ReCaptcha)
[![GitHub Release](https://badgen.net/github/release/griesoft/orchardcore-recaptcha)](https://github.com/griesoft/orchardcore-recaptcha/releases)

_The latest release build of this module is compatible with Orchard Core version `1.8.*`._

## Installation

To install, use the NuGet package manager:

`PM> Install-Package Griesoft.OrchardCore.ReCaptcha`

### Prerequisites
The reCAPTCHA service, provided by Google, requires registration. Sign up [here](http://www.google.com/recaptcha/admin). For more information, refer to [Google's guide](https://developers.google.com/recaptcha/intro#overview).

Post-registration, you will receive a **Site key** and a **Secret key**, necessary for configuring the service in your app.

### Enable the Feature
In the Admin panel, navigate to `Configuration -> Features`. Search for `ReCaptcha` and enable the module provided by us.

### Configuration

Navigate to `Configuration -> Settings -> ReCaptcha`. Enter your Site and Secret keys acquired earlier and click save.

_Hint: You can also configure the service via app settings. For more information, refer to [this guide](https://github.com/griesoft/aspnetcore-recaptcha#settings). This method also supports multi-tenancy._

## Usage
The module includes parts for each reCAPTCHA challenge type (Invisible, V2, V3) that you can attach to your content types. Additionally, it provides widgets, which are particularly useful with the `OrchardCore.Forms` module.

Ensure the part or widget **is rendered inside** the `<form>` element. Depending on the reCAPTCHA type, it may render a submit button for your form, initiating the challenge and submitting the form upon successful completion.

You can customize the post-challenge behavior by specifying a custom JS callback function. In this case, ensure to submit your form within this callback.

Placing the challenge outside a form is possible, but you must ensure the challenge token from reCAPTCHA is included in the form submission. This can be done in three ways:
- As a value in the form request header named `G-Recaptcha-Response`
- As an input within the form named `g-recaptcha-response`
- As a query parameter in the form request named `g-recaptcha-response`

For customizing part or widget templates and accessing tag helpers, add the following to your `_ViewImports.cshtml`:

```razor
@using Griesoft.OrchardCore.ReCaptcha
@addTagHelper *, Griesoft.OrchardCore.ReCaptcha
```

### Validation
This module includes validation tasks for workflows to validate incoming HTTP requests.

For validating requests in code (e.g., in controllers or actions), use the validation attributes from the base package. Refer to the [base repository instructions](https://github.com/griesoft/aspnetcore-recaptcha#adding-backend-validation-to-an-action). Importantly, use the Griesoft.AspNetCore.ReCaptcha namespace for accessing the ValidateRecaptcha attribute and related validation logic and services.
