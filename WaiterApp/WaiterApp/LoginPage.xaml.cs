using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using Infrastructure.ViewModels;
using System;
using Xamarin.Forms;

namespace WaiterApp
{
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseConnectionChecker _databaseConnectionChecker;
        private readonly LoginViewModel _model;
        private bool _connectedToDb = false;
        public LoginPage(DatabaseConnectionChecker databaseConnectionChecker)
        {
            InitializeComponent();
            _databaseConnectionChecker = databaseConnectionChecker;
            _model = new LoginViewModel();
            BindingContext = _model;

            TestConnection();
            _model.LoadParameters();
            if (bool.Parse(ParametersLoader.Parameters["remember"]))
            {
                _model.Username = ParametersLoader.Parameters["username"];
                Login(ParametersLoader.Parameters["password"]);
            }
        }

        protected override void OnAppearing()
        {
            _model.LoadParameters();
        }

        private async void TestConnection()
        {
            var dbConnectionChecker = new DatabaseConnectionChecker();
            try
            {
                dbConnectionChecker.TestConnection();
                _connectedToDb = true;
            }
            catch (ConnectionStringException)
            {
                await DisplayAlert("Db error", "Bad connection string. Configure before proceeding", "OK");

                var settingsViewModel = new SettingsViewModel(_databaseConnectionChecker);
                await Navigation.PushAsync(new SettingsPage(settingsViewModel));
                _connectedToDb = false;
            }
        }

        private void OnSettingsButtonClick(object sender, EventArgs e)
        {
            var settingsViewModel = new SettingsViewModel(_databaseConnectionChecker);
            Navigation.PushAsync(new SettingsPage(settingsViewModel));
        }

        private void OnLoginButtonClick(object sender, EventArgs e)
        {
            if (_model.RememberUser)
            {
                ParametersLoader.SetParameter("remember", "true");
                ParametersLoader.SetParameter("username", _model.Username);
                ParametersLoader.SetParameter("password", PasswordEntry.Text);
            }
            else
            {
                ParametersLoader.SetParameter("remember", "false");
                ParametersLoader.SetParameter("username", string.Empty);
                ParametersLoader.SetParameter("password", string.Empty);
            }
            Login(PasswordEntry.Text);
        }

        private async void Login(string password)
        {
            if(!_connectedToDb)
            {
                return;
            }

            var waiter = _model.Login(password);
            if (waiter != null)
            {
                ParametersLoader.SetParameter("waiterId", waiter.Id.ToString());
                ParametersLoader.SaveParameters();
                var page = new MainPage(new MainPageViewModel(
                    new OrderProductRepository(), new GroupRepository(), new SubgroupRepository(), new ProductRepository(),
                    new TableRepository(), new OrderRepository()));
                await Navigation.PushAsync(page);
            }
            else
            {
                await DisplayAlert("Login error", "Wrong credentials, please retry", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            if (!_model.RememberUser)
            {
                ParametersLoader.SetParameter("remember", _model.RememberUser.ToString());
                ParametersLoader.SaveParameters();
            } 
        }
    }
}
