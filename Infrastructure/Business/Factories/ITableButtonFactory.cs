using Core.Models;
using Infrastructure.Business.ControlsDrawing;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Infrastructure.Business.Factories
{
    public interface ITableButtonFactory
    {
        Button Build(DrawnTable drawnTable);
    }
}