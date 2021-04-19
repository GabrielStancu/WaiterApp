using Infrastructure.Helpers;
using Xamarin.Forms;

namespace WaiterApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var paramLoader = new ParametersLoader();
            var loginPage = new LoginPage(paramLoader);
            MainPage = new NavigationPage(loginPage);
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
