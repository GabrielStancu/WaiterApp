using Core.Models;
using GalaSoft.MvvmLight.Command;
using Infrastructure.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Infrastructure.ViewModels
{
    public class SettingsViewModel: BaseViewModel
    {
        public SettingsViewModel(ParametersLoader loader, bool loadDepartments)
        {
            _loader = loader;
            Nickname = _loader.Parameters["nickname"];
            ServerName = _loader.Parameters["server"];
            DatabaseName = _loader.Parameters["database"];
            DbUser = _loader.Parameters["dbUser"];
            DbPassword = _loader.Parameters["dbPassword"];
            LoadAtStartup = bool.Parse(_loader.Parameters["loadDb"]);
            //LoadDepartments();
            //CrtDepartment = SelectDepartment(_loader.Parameters["department"]);
            if(loadDepartments)
            {
                LoadDepartments();
            }
        }

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                SetProperty<string>(ref _nickname, value);
            }
        }

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();
        private Department _crtDepartment;
        public Department CrtDepartment
        {
            get => _crtDepartment;
            set
            {
                _crtDepartment = value;
                SetProperty<Department>(ref _crtDepartment, value);
            }
        }
        private string _serverName;
        public string ServerName
        {
            get => _serverName;
            set
            {
                _serverName = value;
                SetProperty<string>(ref _serverName, value);
            }
        }
        private string _databaseName;
        public string DatabaseName
        {
            get => _databaseName;
            set
            {
                _databaseName = value;
                SetProperty<string>(ref _databaseName, value);
            }
        }
        private string _dbUser;
        public string DbUser
        {
            get => _dbUser;
            set
            {
                _dbUser = value;
                SetProperty<string>(ref _dbUser, value);
            }
        }
        private string _dbPassword;
        public string DbPassword
        {
            get => _dbPassword;
            set
            {
                _dbPassword = value;
                SetProperty<string>(ref _dbPassword, value);
            }
        }
        private bool _loadAtStartup;
        private readonly ParametersLoader _loader;

        public bool LoadAtStartup
        {
            get => _loadAtStartup;
            set
            {
                _loadAtStartup = value;
                SetProperty<bool>(ref _loadAtStartup, value);
            }
        }

        public ICommand OnSaveParameters 
        {
            get { return new RelayCommand(SaveParameters); }
        }

        private async void LoadDepartments()
        {
            var departmentLoader = new DepartmentLoader();
            var departments = await departmentLoader.LoadAllDepartments();
            departments.ForEach(dep => Departments.Add(dep));
            CrtDepartment = departmentLoader.LoadCurrentDepartment(departments, _loader.Parameters["department"]);
        }

        private void SaveParameters()
        {
            _loader.SetParameter("nickname", Nickname);
            _loader.SetParameter("department", CrtDepartment?.Name);
            _loader.SetParameter("server", ServerName);
            _loader.SetParameter("database", DatabaseName);
            _loader.SetParameter("dbUser", DbUser);
            _loader.SetParameter("dbPassword", DbPassword);
            _loader.SetParameter("loadDb", LoadAtStartup.ToString());
            _loader.SaveParameters();
        }
    }
}
