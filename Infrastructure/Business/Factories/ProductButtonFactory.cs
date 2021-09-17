using Core.Models;
using Xamarin.Forms;

namespace Infrastructure.Business.Factories
{
    public class ProductButtonFactory : IProductButtonFactory
    {
        public Button Build(Product product)
        {
            return new Button
            {
                Text = product.Name,
                BackgroundColor = Color.DarkGray,
                TextColor = Color.White,
                CornerRadius = 20,
                BindingContext = product,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 100
            };
        }
    }
}
