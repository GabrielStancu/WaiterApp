using Infrastructure.Business.ControlsDrawing;
using Xamarin.Forms;

namespace Infrastructure.Business.Factories
{
    public class TableButtonFactory : ITableButtonFactory
    {
        public Button Build(DrawnTable drawnTable)
        {
            var tableMessage = (drawnTable.Total == 0) ? string.Empty : drawnTable.Total.ToString();
            return new Button()
            {
                BackgroundColor = drawnTable.Color,
                Text = $"{drawnTable.WaiterName ?? string.Empty}\n[{drawnTable.TableNumber}]{tableMessage}",
                CornerRadius = 6,
                HeightRequest = drawnTable.LengthY,
                WidthRequest = drawnTable.LengthX,
                BorderWidth = 2,
                BorderColor = Color.Black,
                ClassId = drawnTable.TableNumber.ToString()
            };
        }
    }
}
