using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsViewModel _model;
        public SettingsPage(SettingsViewModel model)
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
                await Navigation.PushAsync(new ParametersPage(new ParametersViewModel(new ParametersLoader())));
            }
            catch(ConnectionStringException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}