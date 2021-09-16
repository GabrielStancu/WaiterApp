using Infrastructure.Exceptions;
using Infrastructure.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly ISettingsViewModel _model;
        public SettingsPage(ISettingsViewModel model)
        {
            InitializeComponent();
            _model = model;
            BindingContext = _model;
        }

        private async void OnSaveParameters(object sender, System.EventArgs e)
        {
            _model.SaveParameters();

            try
            {
                _model.TestConnection();
                await Navigation.PushAsync(App.Container.Resolve<ParametersPage>());
            }
            catch(ConnectionStringException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}