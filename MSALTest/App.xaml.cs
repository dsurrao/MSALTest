using Xamarin.Forms;

namespace MSALTest
{
    public partial class App : Application
    {
        public static MSALPublicClient mSALPublicClient;

        public App()
        {
            InitializeComponent();

            string bundleID = "com.dominicsurrao.MSALTest"; // replace
            string clientID = <Azure AD app registration client ID>;
            string tenantID = <Azure AD tenant ID>;
            string redirectURI = "msauth." + bundleID + "://auth"; 
            string[] scopes = new string[] { "user.read" };

            mSALPublicClient = new MSALPublicClient(bundleID, clientID, tenantID,
                redirectURI, scopes);

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
