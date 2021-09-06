using Core.Models;
using Infrastructure.Helpers;
using System;
using System.Collections.ObjectModel;

namespace Infrastructure.ViewModels
{
    public class ParametersViewModel: BaseViewModel
    {
        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                SetProperty<string>(ref _nickname, value);
            }
        }

        private string _buttonsPerLine;
        public string ButtonsPerLine
        {
            get => _buttonsPerLine;
            set
            {
                _buttonsPerLine = value;
                SetProperty<string>(ref _buttonsPerLine, value);
            }
        }

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();
        private Department _crtDepartment;
        public Department CrtDepartment
        {
            get => _crtDepartment;
            set
            {
                _crtDepartment = value;
                SetProperty<Department>(ref _crtDepartment, value);
            }
        }

        private bool _loadAtStartup;
        public bool LoadAtStartup
        {
            get => _loadAtStartup;
            set
            {
                _loadAtStartup = value;
                SetProperty<bool>(ref _loadAtStartup, value);
            }
        }

        private readonly ParametersLoader _loader;

        public ParametersViewModel(ParametersLoader loader)
        {  
            _loader = loader;
            LoadDepartments();
            LoadParameters();
        }

        private void LoadDepartments()
        {
            var departmentLoader = new DepartmentLoader();
            var departments = departmentLoader.LoadAllDepartments();
            departments.ForEach(dep => Departments.Add(dep));

            if(Int32.TryParse(_loader.Parameters["departmentId"], out int departmentId))
            {
                CrtDepartment = departmentLoader.LoadCurrentDepartment(departments, departmentId);
            }
        }

        private void LoadParameters()
        {
            Nickname = _loader.Parameters["nickname"];
            ButtonsPerLine = _loader.Parameters["buttonsPerLine"];
        }

        public void SaveParameters()
        {
            _loader.SetParameter("nickname", Nickname);
            _loader.SetParameter("departmentId", CrtDepartment?.Id.ToString());
            _loader.SetParameter("loadDb", LoadAtStartup.ToString());
            _loader.SetParameter("buttonsPerLine", ButtonsPerLine.ToString());
            _loader.SaveParameters();
        }
    }
}
