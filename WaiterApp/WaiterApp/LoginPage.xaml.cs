using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.ViewModels;
using System;
using Xamarin.Forms;

namespace WaiterApp
{
    public partial class LoginPage : ContentPage
    {
        private readonly ParametersLoader _parametersLoader;

        public LoginPage(ParametersLoader parametersLoader)
        {
            InitializeComponent();
            _parametersLoader = parametersLoader;
            TestConnection();

            this.BindingContext = new LoginViewModel(parametersLoader);
        }

        private async void TestConnection()
        {
            var dbConnectionChecker = new DatabaseConnectionChecker();
            try
            {
                dbConnectionChecker.TestConnection();             
            }
            catch(ConnectionStringException)
            {
                await DisplayAlert("Db error", "Bad connection string. Configure before proceeding", "OK");
                var settingsViewModel = new SettingsViewModel(_parametersLoader, false);
                await Navigation.PushAsync(new SettingsPage(settingsViewModel));
            }
        }

        private void OnSettingsButtonClick(object sender, EventArgs e)
        {
            var settingsViewModel = new SettingsViewModel(_parametersLoader, true);
            Navigation.PushAsync(new SettingsPage(settingsViewModel));
        }

        private void OnLoginButtonClick(object sender, EventArgs e)
        {

        }

        private void OnRememberUserCheckboxCheck(object sender, CheckedChangedEventArgs e)
        {

        }
    }
}
