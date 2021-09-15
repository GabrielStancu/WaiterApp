using Core.Models;
using Infrastructure.Business.Parameters;
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

        public ParametersViewModel()
        {  
            LoadDepartments();
            LoadParameters();
        }

        private void LoadDepartments()
        {
            var departmentLoader = new DepartmentLoader();
            var departments = departmentLoader.LoadAllDepartments();
            departments.ForEach(dep => Departments.Add(dep));

            if(Int32.TryParse(ParametersLoader.Parameters[AppParameters.DepartmentId], out int departmentId))
            {
                CrtDepartment = departmentLoader.LoadCurrentDepartment(departments, departmentId);
            }
        }

        private void LoadParameters()
        {
            Nickname = ParametersLoader.Parameters[AppParameters.Nickname];
            ButtonsPerLine = ParametersLoader.Parameters[AppParameters.ButtonsPerLine];
        }

        public bool SaveParameters()
        {
            if (string.IsNullOrEmpty(Nickname) ||
                CrtDepartment is null ||
                string.IsNullOrEmpty(ButtonsPerLine))
            {
                return false;
            }

            ParametersLoader.SetParameter(AppParameters.Nickname, Nickname);
            ParametersLoader.SetParameter(AppParameters.DepartmentId, CrtDepartment.Id.ToString());
            ParametersLoader.SetParameter(AppParameters.LoadDb, LoadAtStartup.ToString());
            ParametersLoader.SetParameter(AppParameters.ButtonsPerLine, ButtonsPerLine);
            ParametersLoader.SaveParameters();

            return true;
        }
    }
}
