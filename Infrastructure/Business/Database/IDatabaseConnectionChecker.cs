namespace Infrastructure.Business.Database
{
    public interface IDatabaseConnectionChecker
    {
        bool TestConnection();
    }
}