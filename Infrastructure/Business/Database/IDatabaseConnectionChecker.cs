namespace Infrastructure.Business.Database
{
    public interface IDatabaseConnectionChecker
    {
        void TestConnection();
    }
}