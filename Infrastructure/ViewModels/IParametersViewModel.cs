using Core.Models;
using System.Collections.ObjectModel;

namespace Infrastructure.ViewModels
{
    public interface IParametersViewModel
    {
        string ButtonsPerLine { get; set; }
        Department CrtDepartment { get; set; }
        ObservableCollection<Department> Departments { get; set; }
        bool LoadAtStartup { get; set; }
        string Nickname { get; set; }

        bool SaveParameters();
    }
}