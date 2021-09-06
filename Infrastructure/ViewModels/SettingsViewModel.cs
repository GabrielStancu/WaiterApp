using Infrastructure.Helpers;

namespace Infrastructure.ViewModels
{
    public class SettingsViewModel: BaseViewModel
    {
        public SettingsViewModel(ParametersLoader loader, DatabaseConnectionChecker checker)
        {
            _loader = loader;
            _checker = checker;
            ServerName = _loader.Parameters["server"];
            DatabaseName = _loader.Parameters["database"];
            DbUser = _loader.Parameters["dbUser"];
            DbPassword = _loader.Parameters["dbPassword"];
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
        
        private readonly ParametersLoader _loader;
        private readonly DatabaseConnectionChecker _checker;

        public void SaveParameters()
        {
            _loader.SetParameter("server", ServerName);
            _loader.SetParameter("database", DatabaseName);
            _loader.SetParameter("dbUser", DbUser);
            _loader.SetParameter("dbPassword", DbPassword);
            _loader.SaveParameters();
        }

        public void TestConnection()
        {
            _checker.TestConnection();
        }
    }
}
