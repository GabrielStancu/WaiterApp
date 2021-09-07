using Infrastructure.Helpers;

namespace Infrastructure.ViewModels
{
    public class SettingsViewModel: BaseViewModel
    {
        public SettingsViewModel(DatabaseConnectionChecker checker)
        {
            _checker = checker;
            ServerName = ParametersLoader.Parameters["server"];
            DatabaseName = ParametersLoader.Parameters["database"];
            DbUser = ParametersLoader.Parameters["dbUser"];
            DbPassword = ParametersLoader.Parameters["dbPassword"];
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
        
        private readonly DatabaseConnectionChecker _checker;

        public void SaveParameters()
        {
            ParametersLoader.SetParameter("server", ServerName);
            ParametersLoader.SetParameter("database", DatabaseName);
            ParametersLoader.SetParameter("dbUser", DbUser);
            ParametersLoader.SetParameter("dbPassword", DbPassword);
            ParametersLoader.SaveParameters();
        }

        public void TestConnection()
        {
            _checker.TestConnection();
        }
    }
}
