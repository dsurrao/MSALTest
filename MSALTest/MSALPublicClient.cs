using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace MSALTest
{
    public class MSALPublicClient
    {
        IPublicClientApplication pca;
        string _bundleID;
        string _clientID;
        string _tenantID;
        string _redirectURI;
        string[] _scopes;

        static readonly HttpClient _client = new HttpClient();

        public MSALPublicClient(string bundleID, string clientID,
            string tenantID, string redirectURI, string[] scopes)
        {
            // LaserMDXamarin App registration in Azure AD
            _bundleID = bundleID;
            _clientID = clientID;
            _tenantID = tenantID;
            _redirectURI = redirectURI;
            _scopes = scopes;

            pca = PublicClientApplicationBuilder.Create(_clientID)
                .WithAuthority(AzureCloudInstance.AzurePublic, _tenantID)
                .WithRedirectUri(_redirectURI)
                .WithIosKeychainSecurityGroup(_bundleID)
                .Build();
        }

        public async Task<string> Login()
        {
            string userInfo = "";

            // get available accounts from user token cache
            var accounts = await pca.GetAccountsAsync();

            AuthenticationResult result;

            try
            {
                // try to acquire token silently
                result = await pca.AcquireTokenSilent(_scopes,
                    accounts.FirstOrDefault()).ExecuteAsync();
            }
            catch(MsalUiRequiredException)
            {
                try
                {
                    // if not, try interactive authentication session
                    result = await pca.AcquireTokenInteractive(
                        _scopes)
                        .WithPrompt(Prompt.ForceLogin)
                        .ExecuteAsync();

                    // now query the graph api using the access token
                    if (result != null)
                    {
                        try
                        {
                            string endpoint = "https://graph.microsoft.com/v1.0/me";

                            HttpRequestMessage request = new HttpRequestMessage(
                                HttpMethod.Get, endpoint);

                            request.Headers.Authorization =
                                new System.Net.Http.Headers.AuthenticationHeaderValue(
                                    "Bearer", result.AccessToken);

                            HttpResponseMessage response = await _client.SendAsync(
                                request);

                            userInfo = await response.Content.ReadAsStringAsync();
                        }
                        catch (HttpRequestException e)
                        {
                            Console.WriteLine("Exception: {0}", e.Message);
                        }
                    }
                }
                catch(MsalClientException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return userInfo;
        }

        public async Task Logout()
        {
            // get available accounts from user token cache
            var accounts = await pca.GetAccountsAsync();
            while (accounts.Any())
            {
                await pca.RemoveAsync(accounts.FirstOrDefault());
                accounts = await pca.GetAccountsAsync();
            }
        }
    }
}
