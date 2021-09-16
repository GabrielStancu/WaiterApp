namespace Infrastructure.ViewModels
{
    public interface ISettingsViewModel
    {
        string DatabaseName { get; set; }
        string DbPassword { get; set; }
        string DbUser { get; set; }
        string ServerName { get; set; }

        void SaveParameters();
        void TestConnection();
    }
}