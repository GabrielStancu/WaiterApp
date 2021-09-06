using Core.Models;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using System;

namespace Infrastructure.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ParametersLoader _parametersLoader;

        public LoginViewModel(ParametersLoader parametersLoader)
        {
            _parametersLoader = parametersLoader; 
            LoadParameters();
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

        private int _departmentId;
        public int DepartmentId 
        { 
            get { return _departmentId; } 
            set
            {
                _departmentId = value;
                SetProperty<int>(ref _departmentId, value);
            }
        }

        private Department _department;
        public Department Department
        {
            get { return _department; }
            set
            {
                _department = value;
                SetProperty<Department>(ref _department, value);
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

        public void LoadParameters()
        {
            Nickname = _parametersLoader.Parameters["nickname"];
            DepartmentId = int.Parse(_parametersLoader.Parameters["departmentId"]);
            Username = _parametersLoader.Parameters["username"];
            Password = _parametersLoader.Parameters["password"];
            RememberUser = bool.Parse(_parametersLoader.Parameters["remember"]);
            CurrentDate = DateTime.Today;

            if (DepartmentId != 0)
            {
                Department = new DepartmentRepository().SelectById(DepartmentId);
            }
        }

        public Waiter Login(string password)
        {
            var waiterRepository = new WaiterRepository();
            var user = waiterRepository.SelectWaiterWithCredentials(Username, password);

            return user;
        }
    }
}
