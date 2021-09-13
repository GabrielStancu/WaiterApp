using Infrastructure.Helpers.Database;
using Infrastructure.Helpers.Parameters;
using Xamarin.Forms;

namespace WaiterApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ParametersLoader.InitParameters();
            ParametersLoader.LoadParameters();
            var connectionChecker = new DatabaseConnectionChecker();
            new ContextConnectionStringSetter().SetConnectionString();

            var loginPage = new LoginPage(connectionChecker);
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
