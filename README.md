# Orchard Core reCAPTCHA Module
An Orchard Core reCAPTCHA module that is based on our [ASP.NET Core reCAPTCHA](https://github.com/jgdevlabs/aspnetcore-recaptcha) service.

All features are included in this module from the base service.

[![Build Status](https://dev.azure.com/griesingersoftware/Orchard%20Core%20reCAPTCHA%20Module/_apis/build/status/CI%20Pipeline?branchName=main)](https://dev.azure.com/griesingersoftware/Orchard%20Core%20reCAPTCHA%20Module/_apis/build/status/CI%20Pipeline?branchName=main)
[![Build Status](https://vsrm.dev.azure.com/griesingersoftware/_apis/public/Release/badge/a7959783-e730-4a16-8ec8-436620f88501/1/2)](https://vsrm.dev.azure.com/griesingersoftware/_apis/public/Release/badge/a7959783-e730-4a16-8ec8-436620f88501/1/2)
[![License](https://badgen.net/github/license/jgdevlabs/orchardcore-recaptcha)](https://github.com/jgdevlabs/orchardcore-recaptcha/blob/master/LICENSE)
[![NuGet](https://badgen.net/nuget/v/Griesoft.OrchardCore.ReCaptcha)](https://www.nuget.org/packages/Griesoft.OrchardCore.ReCaptcha)
[![GitHub Release](https://badgen.net/github/release/jgdevlabs/orchardcore-recaptcha)](https://github.com/jgdevlabs/orchardcore-recaptcha/releases)

## Work in progress
This module is still work in progress.

**Features**

- [x] Site and secret key settings
- [x] Port tag helpers. Modify the script tag helper to register the output to the resource manager
- [x] Validation workflows for V2 and V3
- [ ] Add a content part and a widget
- [ ] Add content part settings

The module is already usable, but you need to add the challenge into your templates with the help of the provided tag helpers.

## Installation

Install in your web project via [NuGet](https://www.nuget.org/packages/Griesoft.OrchardCore.ReCaptcha/) using:

`PM> Install-Package Griesoft.OrchardCore.ReCaptcha`

## Usage

### Prequisites
You will need an API key pair which can be acquired by [signing up here](http://www.google.com/recaptcha/admin). For assistance or other questions regarding that topic, refer to [Google's guide](https://developers.google.com/recaptcha/intro#overview).

After sign-up, you should have a **Site key** and a **Secret key**. You will need those to configure the service in your app.

### Enable the Feature
In the Admin panel, navigate to `Configuration -> Features`. Search for `ReCaptcha` and enable the module that is provided by us.

### Configuration

#### From the Admin Panel
Navigate to `Configuration -> Settings -> ReCaptcha`. Now enter your Site and Secret key that you acquired earlier and hit save.

#### From the appsettings.json
There are two ways to add the site and secret key to your `appsettings.json` file. 

One is by adding the configuration to the root of the file like described [here](https://github.com/jgdevlabs/aspnetcore-recaptcha#settings).

The other is by adding the configuration inside the `OrchardCore` section. This means you will be also able to configure reCAPTCHA for multible tenants differently. An example of the latter:

```json
"OrchardCore": {
    "RecaptchaSettings": {
        "SiteKey": "<Your site key goes here>",
        "SecretKey": "<Your secret key goes here>"
    }
}
```

### Adding reCAPTCHA to your template
The content part for this module is still in development, but you may add a challenge to your views with the existing tag helpers in the meanwhile.

Add the following to your `_ViewImports.cshtml`:

```razor
@using Griesoft.OrchardCore.ReCaptcha
@addTagHelper *, Griesoft.OrchardCore.ReCaptcha
```

First, add `<recaptcha-script />` to the view that you want to add a challenge. That will add the necessary script to the foot of your page.

Now for a simple reCAPTCHA V2 checkbox, you would add `<recaptcha />` inside your form. If you want invisible reCAPTCHA add `<button re-invisible form-id="yourFormId">Submit</button>` to your form.

In case of reCAPTCHA V3 modify the recaptcha script tag like this `<recaptcha-script render="V3" />` and use the `<recaptcha-v3 form-id="yourFormId" action="submit">Submit</recaptcha-v3>` tag inside of your form.

### Challenge Validation
If you make use of workflows, there are tasks that you may use to validate incoming HTTP requests.

If you need to validate incoming requests to your controllers or actions, you may refer to the validation [documentation of the base repo](https://github.com/jgdevlabs/aspnetcore-recaptcha#adding-backend-validation-to-an-action). One important thing to note is that you would make use of the `Griesoft.AspNetCore.ReCaptcha` namespace. That contains the `ValidateRecaptcha` attribute and all other validation-related logic and services.