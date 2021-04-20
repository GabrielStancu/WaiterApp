using Core.Models;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ParametersLoader _parametersLoader;

        public LoginViewModel(ParametersLoader parametersLoader)
        {
            _parametersLoader = parametersLoader;
            Nickname = _parametersLoader.Parameters["nickname"];
            Department = _parametersLoader.Parameters["department"];
            Username = _parametersLoader.Parameters["username"];
            Password = _parametersLoader.Parameters["password"];
            RememberUser = bool.Parse(_parametersLoader.Parameters["remember"]);
            CurrentDate = DateTime.Today;
        }

        private string _nickname;
        public string Nickname 
        {
            get { return _nickname; }
            set
            {
                _nickname = value;
                SetProperty<string>(ref _nickname, value);
            }
        }

        private string _department;
        public string Department 
        { 
            get { return _department; } 
            set
            {
                _department = value;
                SetProperty<string>(ref _department, value);
            }
        }

        private string _username;
        public string Username 
        {
            get { return _username; }
            set
            {
                _username = value;
                SetProperty<string>(ref _username, value); 
            }
        }

        private string _password;
        public string Password 
        {
            get { return string.Empty; }
            set
            {
                _password = value;
                SetProperty<string>(ref _password, value);
            }
        }

        private bool _rememberUser;
        public bool RememberUser 
        {
            get { return _rememberUser; }
            set
            {
                _rememberUser = value;
                SetProperty<bool>(ref _rememberUser, value);
            }
        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                SetProperty<DateTime>(ref _currentDate, value);
            }
        }

        public async Task<Waiter> LoginAsync(string password)
        {
            var waiterRepository = new WaiterRepository();
            var user = await waiterRepository.SelectWaiterWithCredentialsAsync(Username, password);

            return user;
        }
    }
}
