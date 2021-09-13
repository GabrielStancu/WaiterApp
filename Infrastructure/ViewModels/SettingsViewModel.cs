using Infrastructure.Helpers.Database;
using Infrastructure.Helpers.Parameters;

namespace Infrastructure.ViewModels
{
    public class SettingsViewModel: BaseViewModel
    {
        public SettingsViewModel(DatabaseConnectionChecker checker)
        {
            _checker = checker;
            ServerName = ParametersLoader.Parameters[AppParameters.Server];
            DatabaseName = ParametersLoader.Parameters[AppParameters.Database];
            DbUser = ParametersLoader.Parameters[AppParameters.DbUser];
            DbPassword = ParametersLoader.Parameters[AppParameters.DbPassword];
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
            ParametersLoader.SetParameter(AppParameters.Server, ServerName);
            ParametersLoader.SetParameter(AppParameters.Database, DatabaseName);
            ParametersLoader.SetParameter(AppParameters.DbUser, DbUser);
            ParametersLoader.SetParameter(AppParameters.DbPassword, DbPassword);
            ParametersLoader.SaveParameters();
        }

        public void TestConnection()
        {
            _checker.TestConnection();
        }
    }
}
