using Infrastructure.Exceptions;
using Infrastructure.Business.Database;
using Infrastructure.Business.Parameters;
using Infrastructure.ViewModels;
using System;
using Xamarin.Forms;

namespace WaiterApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly ILoginViewModel _model;
        private readonly IDatabaseConnectionChecker _databaseConnectionChecker;
        private bool _connectedToDb = false;
        public LoginPage(ILoginViewModel model, IDatabaseConnectionChecker databaseConnectionChecker)
        {
            InitializeComponent();
            _model = model;
            _databaseConnectionChecker = databaseConnectionChecker;
            BindingContext = _model;

            TestConnection();
        }

        protected async override void OnAppearing()
        {
            if (!_connectedToDb)
            {
                await DisplayAlert("Database error", "Bad connection string. Please configure it before proceeding.", "OK");
                await Navigation.PushAsync(App.Container.Resolve<SettingsPage>());
                _connectedToDb = true;

                return;
            }

            _model.LoadParameters();
        }

        private void TestConnection()
        {
            try
            {
                _connectedToDb = _databaseConnectionChecker.TestConnection();
               
                if (_connectedToDb)
                {
                    _model.LoadParameters();
                    if (bool.Parse(ParametersLoader.Parameters[AppParameters.Remember]))
                    {
                        _model.Username = ParametersLoader.Parameters[AppParameters.Username];
                        PasswordEntry.Text = ParametersLoader.Parameters[AppParameters.Password];
                        Login(ParametersLoader.Parameters[AppParameters.Password]);
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
