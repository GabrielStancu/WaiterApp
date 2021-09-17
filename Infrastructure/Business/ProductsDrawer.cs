using Core.Models;
using Infrastructure.Business.Factories;
using Infrastructure.Business.Parameters;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Infrastructure.Business
{
    public class ProductsDrawer : IProductsDrawer
    {
        private readonly IProductButtonFactory _productButtonFactory;
        public ProductsDrawer(IProductButtonFactory productButtonFactory)
        {
            _productButtonFactory = productButtonFactory;
        }
        public void Draw(Grid productsGrid, IList<Product> products)
        {
            productsGrid.Children?.Clear();
            int productsPerRow = Int32.Parse(ParametersLoader.Parameters[AppParameters.ButtonsPerLine]);
            int rows = products.Count / productsPerRow;
            int crtRow = 0, crtCol = 0;

            if (products.Count % productsPerRow != 0)
            {
                rows++;
            }

            productsGrid.RowDefinitions = new RowDefinitionCollection();
            productsGrid.ColumnDefinitions = new ColumnDefinitionCollection();

            for (int i = 0; i < productsPerRow; i++)
            {
                productsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < rows; i++)
            {
                productsGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < products.Count; i++)
            {
                var productBtn = _productButtonFactory.Build(products[i]);

                productsGrid.Children.Add(productBtn, crtCol++, crtRow);
                if (crtCol == productsPerRow)
                {
                    crtCol = 0;
                    crtRow++;
                }
            }
        }
    }
}
