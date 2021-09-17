using Core.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Infrastructure.Business
{
    public interface IProductsDrawer
    {
        void Draw(Grid productsGrid, IList<Product> products);
    }
}