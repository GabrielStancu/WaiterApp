using Core.Context;
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
            RestaurantContext.ConnectionString = InitConnectionString(paramLoader);

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

        private string InitConnectionString(ParametersLoader paramLoader)
        {
            var server = paramLoader.Parameters["server"];
            var database = paramLoader.Parameters["database"];
            var user = paramLoader.Parameters["dbUser"];
            var password = paramLoader.Parameters["dbPassword"];
            
            var connStrBuilder = new ConnectionStringBuilder(server, database, user, password);
            return connStrBuilder.Build();
        }
    }
}
