using Auth0.OidcClient;
using IdentityModel.OidcClient;

namespace EnchantedAirAppMaui
{
    public partial class MainPage : ContentPage
    {
        private LoginResult loginResult;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            if (loginResult == default)
            {
                var client = new Auth0Client(new Auth0ClientOptions
                {
                    Domain = "dev-0revwfmqbhtkb7m4.us.auth0.com",
                    ClientId = "dev-0revwfmqbhtkb7m4.us.auth0.com",
                    RedirectUri = "myapp://callback",
                    PostLogoutRedirectUri = "myapp://callback",
                    Scope = "openid profile email"
                });
                loginResult = await client.LoginAsync();
                Login.Text = "&Logout";
            }

        }

       
    }

}
