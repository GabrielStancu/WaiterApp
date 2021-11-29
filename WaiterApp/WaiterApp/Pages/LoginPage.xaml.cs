using Infrastructure.Exceptions;
using Infrastructure.Business.Database;
using Infrastructure.Business.Parameters;
using Infrastructure.ViewModels;
using System;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Threading.Tasks;

namespace WaiterApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _model;
        private readonly IDatabaseConnectionChecker _databaseConnectionChecker;
        private bool _connectedToDb = false;
        private bool _appStart = true;
        public LoginPage(LoginViewModel model, IDatabaseConnectionChecker databaseConnectionChecker)
        {
            InitializeComponent();
            _model = model;
            _databaseConnectionChecker = databaseConnectionChecker;
            BindingContext = _model;
        }

        protected async override void OnAppearing()
        {
            await TestConnection();
            if (!_connectedToDb)
            {
                await DisplayAlert("Database error", "No wifi or bad connection string. Check your wifi connection before setting the connection string.", "OK");
                await Navigation.PushAsync(App.Container.Resolve<SettingsPage>());
                _connectedToDb = true;

                return;
            }
            _model.LoadParameters();
        }

        private async Task TestConnection()
        {
            try
            {
                using (UserDialogs.Instance.Loading("Testing database connection..."))
                {
                    _connectedToDb = await Task.Run(() => _databaseConnectionChecker.TestConnection());
                    if (_connectedToDb)
                    {
                        _model.LoadParameters();
                        if (bool.Parse(ParametersLoader.Parameters[AppParameters.Remember]) && _appStart)
                        {
                            _appStart = false;
                            _model.Username = ParametersLoader.Parameters[AppParameters.Username];
                            PasswordEntry.Text = ParametersLoader.Parameters[AppParameters.Password];
                            Login(ParametersLoader.Parameters[AppParameters.Password]);
                        }
                    }
                }
            }
            catch (ConnectionStringException)
            {
                _connectedToDb = false;   
            }
        }

        private void OnSettingsButtonClick(object sender, EventArgs e)
        {
            Navigation.PushAsync(App.Container.Resolve<SettingsPage>());
        }

        private void OnLoginButtonClick(object sender, EventArgs e)
        {
            if (_model.RememberUser)
            {
                ParametersLoader.SetParameter(AppParameters.Remember, true.ToString());
                ParametersLoader.SetParameter(AppParameters.Username, _model.Username);
                ParametersLoader.SetParameter(AppParameters.Password, PasswordEntry.Text);
            }
            else
            {
                ParametersLoader.SetParameter(AppParameters.Remember, false.ToString());
                ParametersLoader.SetParameter(AppParameters.Username, string.Empty);
                ParametersLoader.SetParameter(AppParameters.Password, string.Empty);
            }
            Login(PasswordEntry.Text);
        }

        private async void Login(string password)
        {
            if(!_connectedToDb)
            {
                return;
            }

            try
            {
                var waiter = _model.Login(password);
                if (waiter != null)
                {
                    ParametersLoader.SetParameter(AppParameters.WaiterId, waiter.Id.ToString());
                    ParametersLoader.SaveParameters();
                    await Navigation.PushAsync(App.Container.Resolve<MainPage>());

                    if (!_model.RememberUser)
                    {
                        PasswordEntry.Text = string.Empty;
                    }
                }
                else
                {
                    await DisplayAlert("Login error", "Wrong credentials, please retry", "OK");
                }
            }
            catch(WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override void OnDisappearing()
        {
            if (!_model.RememberUser)
            {
                ParametersLoader.SetParameter(AppParameters.Remember, _model.RememberUser.ToString());
                ParametersLoader.SaveParameters();
            } 
        }
    }
}
