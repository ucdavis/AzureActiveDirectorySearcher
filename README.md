# Azure ActiveDirectory Searcher

Simple library for easily searching users in AzureAD via the GraphAPI.  Handles the application authentication for you using direct application authentication instead of delegated user auth.

## Setup

First you need to create an AzureAD application, then collect the following information:

1. Tenant Name: This is the domain name of your AzureAD instance, usually something.onmicrosoft.com.
2. Tenant ID: You can get this by visiting https://login.windows.net/[TENANTNAME]/.well-known/openid-configuration and finding the GUID portion of the token_endpoint property.
3. Client ID: On the "configure" page of your application. It's a GUID.
4. Client Secret/Key: Also on the "configure" page, you need to create a Key and then save it.

**Important:** you also need to grant this application permission to read directory data. On the configure page under "permissions to other applications" make sure "application permissions" "read directory data" is selected.  That should be all you need to do, but if you get permissions errors you can also add the graphAPI application to that list.

## Notes

Has some specific queries based around UC Davis "kerberos" identifiers, but will work for any AzureAD instance and gives you the underlying authenticated ActiveDirectoryClient which you can use directly to query all sorts of stuff.

## Other reading

I found https://github.com/Azure-Samples/active-directory-dotnet-graphapi-console helpful and it has many different examples of graphAPI queries you can do and includes additional authentication options.
