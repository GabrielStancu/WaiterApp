using Core.Models;
using System;

namespace Infrastructure.ViewModels
{
    public interface ILoginViewModel
    {
        DateTime CurrentDate { get; set; }
        Department Department { get; set; }
        int DepartmentId { get; set; }
        string Nickname { get; set; }
        string Password { get; set; }
        bool RememberUser { get; set; }
        string Username { get; set; }

        void LoadParameters();
        Waiter Login(string password);
    }
}