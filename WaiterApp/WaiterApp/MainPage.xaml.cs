using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage(Waiter waiter)
        {
            InitializeComponent();
            DisplayWaiter(waiter);
        }

        private async void DisplayWaiter(Waiter waiter)
        {
            await DisplayAlert("", $"Welcome, {waiter.FirstName} {waiter.LastName}!", "OK");
        }
    }
}