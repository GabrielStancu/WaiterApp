namespace Infrastructure.Business.Database
{
    public interface IConnectionStringBuilder
    {
        string Build(string server, string database, string user, string password);
    }
}