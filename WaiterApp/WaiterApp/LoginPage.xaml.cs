using Infrastructure.Helpers;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void OnSettingsButtonClick(object sender, EventArgs e)
        {
            var settingsViewModel = new SettingsViewModel(_parametersLoader);
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
