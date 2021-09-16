using System.ComponentModel;

namespace Infrastructure.ViewModels
{
    public interface IBaseViewModel
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
}