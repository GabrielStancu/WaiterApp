using Core.Models;
using Xamarin.Forms;

namespace Infrastructure.Business.Factories
{
    public interface IProductButtonFactory
    {
        Button Build(Product product);
    }
}