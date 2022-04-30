# Orchard Core reCAPTCHA Module
An Orchard Core reCAPTCHA module that is based on our [ASP.NET Core reCAPTCHA](https://github.com/jgdevlabs/aspnetcore-recaptcha) service.

All features are included in this module from the base service.

## Work in progress
This module is still work in progress.

**Features**

- [x] Site and secret key settings
- [x] Port tag helpers. Modify the script tag helper to register the output to the resource manager
- [x] Validation workflows for V2 and V3
- [ ] Add a content part and a widget
- [ ] Add content part settings

The module is already usable, but you need to add the challenge into your templates with the help of the provided tag helpers.