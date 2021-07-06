using Core.Models;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        private readonly MainPageViewModel _mainPageViewModel;

        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            _mainPageViewModel = mainPageViewModel;
            BindingContext = _mainPageViewModel;

            CurrentPageChanged += OnMainPageCurrentPageChanged;
            LoadOrders();
        }

        private void OnMainPageCurrentPageChanged(object sender, EventArgs e)
        {
            var tabbedPage = (TabbedPage)sender;
            var title = tabbedPage.CurrentPage.Title;

            switch(title)
            {
                case "Orders":
                    LoadOrders();
                    break;
                case "Tables":
                    LoadTables();
                    break;
                case "Products":
                    LoadProducts();
                    break;

            }
        }

        private async void LoadOrders()
        {
            var waiterId = Int32.Parse(Preferences.Get("waiterId", "0"));
            await _mainPageViewModel.LoadOrdersForWaiterAsync(waiterId);
        }

        public async void LoadTables()
        {

        }

        public async void LoadProducts()
        {
            var departmentId = 1;// Int32.Parse(Preferences.Get("departmentId", "0"));
            await _mainPageViewModel.LoadProductsAsync(departmentId);
        }
    }
}