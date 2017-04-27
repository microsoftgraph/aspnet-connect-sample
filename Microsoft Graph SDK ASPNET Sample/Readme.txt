Microsoft Graph SDK ASPNET Sample

This sample project demonstrate how to use Office 365 services via the Microsoft Graph API and ADAL, registering and configuring the application in the AAD v1 Endpoint.
This is a modified version of the "Microsoft Graph REST ASPNET Connect" sample in GitHub: https://github.com/microsoftgraph/aspnet-connect-rest-sample/tree/last_v1_auth

=========================================================================
Configuring the project:

The project needs to be configured using the Office 365 Connected Service to grant permission for the following MS Graph API scopes:
Mail: Send mail as you
User: Sign you in and read your profile

After the project is configured and run, you will need to authenticate using your Azure Tenant credentials and authorize MS Graph to access the configured resources.
Then you can get your email address and send an email using it.

=========================================================================
The future:
In the future, ADAL will be replaced with MSAL and the AAD App v2 endpoint will be used instead.
