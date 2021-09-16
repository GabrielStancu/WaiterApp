using Infrastructure.Business.Database;
using Infrastructure.Business.Parameters;
using Nancy.TinyIoc;
using WaiterApp.Pages;
using WaiterApp.Utils;
using Xamarin.Forms;

namespace WaiterApp
{
    public partial class App : Application
    {
        public static TinyIoCContainer Container;
        public App()
        {
            InitializeComponent();

            ParametersLoader.InitParameters();
            ParametersLoader.LoadParameters();

            Container = new TinyIoCContainer();
            Container.RegisterApp();
            Container.Resolve<IContextConnectionStringSetter>().SetConnectionString();
            MainPage = new NavigationPage(Container.Resolve<LoginPage>());
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
