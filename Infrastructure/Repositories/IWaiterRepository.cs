using Core.Models;

namespace Infrastructure.Repositories
{
    public interface IWaiterRepository: IGenericRepository<Waiter>
    {
        Waiter SelectWaiterWithCredentials(string username, string password);
    }
}