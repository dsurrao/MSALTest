# Summary
This app demonstrates usage of the MSAL library to authenticate against Azure AD, and then using the access token to query the Microsoft Graph API for logged-in user information.

Works on iOS only, use similar principles for Android.

Based on the more detailed example here:
https://github.com/Azure-Samples/active-directory-xamarin-native-v2/tree/master/1-Basic

**WARNING: This code will not compile until you fill out the settings in App.xaml.cs described below.**

# Dependencies
Xamarin.Forms
Microsoft.Identity.Client

# Steps to customize the app with your settings
Enter your own Azure AD settings in App.xaml.cs
```
string bundleID = <iOS bundle identifier in MSALTest.iOS - Info.plist>;
string clientID = <create an app registration in your Azure AD instance and use the generated clientID>;
string tenantID = <Azure AD instance tenant ID>;
string redirectURI = <add this to the redirectURI list for your app registration>;
```

## iOS specific considerations (taken from detailed example link above)

The UserDetailsClient.iOS project only requires one extra line, in AppDelegate.cs. You need to ensure that the OpenUrl handler looks as the snippet below:
```
public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
{
 AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
 return true;
}
```

Once again, this logic is meant to ensure that once the interactive portion of the authentication flow is concluded, the flow goes back to MSAL.

Also, in order to make the token cache work and have the AcquireTokenSilentAsync work multiple steps must be followed :

Enable Keychain access in your Entitlements.plist file and specify in the Keychain Groups your bundle identifier.

In your project options, on iOS Bundle Signing view, select your Entitlements.plist file for the Custom Entitlements field.
When signing a certificate, make sure XCode uses the same Apple Id.

## Configure the iOS project with your apps' return URI (taken from detailed example link above)

Open the UserDetailsClient.iOS\AppDelegate.cs file.

Open the UserDetailsClient.iOS\info.plist file in a text editor (opening it in Visual Studio won't work for this step as you need to edit the text)

In the URL types section, add an entry for the authorization schema used in your redirectUri:

```
<key>CFBundleURLTypes</key>
<array>
  <dict>
    <key>CFBundleTypeRole</key>
    <string>Editor</string>
    <key>CFBundleURLName</key>
    <string>com.dominicsurrao.MSALTest</string>
    <key>CFBundleURLSchemes</key>
    <array>
      <string>msauth.com.dominicsurrao.MSALTest</string>
    </array>
  </dict>
</array>
```

Replace `CFBundleURLName` and `CFBundleURLSchemes` with your settings.
