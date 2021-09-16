using Core.Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        void Delete(T entity);
        void Insert(T entity);
        List<T> SelectAll();
        T SelectById(int id);
        void Update(T entity);
    }
}