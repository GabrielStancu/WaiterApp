using Acr.UserDialogs;
using Infrastructure.Business.Wifi;
using Infrastructure.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsViewModel _model;
        private readonly IWifiConnectionChecker _wifiConnectionChecker;

        public SettingsPage(
            SettingsViewModel model,
            IWifiConnectionChecker wifiConnectionChecker)
        {
            InitializeComponent();
            _model = model;
            _wifiConnectionChecker = wifiConnectionChecker;
            BindingContext = _model;
        }

        private async void OnSaveParameters(object sender, System.EventArgs e)
        {
            _model.SaveParameters();

            using (UserDialogs.Instance.Loading("Testing database connection..."))
            {
                if(_wifiConnectionChecker.CheckConnection() != WifiConnectionResponse.WIFI_DATA_INTERNET)
                {
                    await DisplayAlert("Error", "Not connected to wifi network. Connect before retrying.", "OK");
                    return;
                }

                bool connected = await Task.Run(() => _model.TestConnection());
                if (connected)
                {
                    await Navigation.PushAsync(App.Container.Resolve<ParametersPage>());
                }
                else
                {
                    await DisplayAlert("Error", "Bad connection string.", "OK");
                }
            }
        }
    }
}