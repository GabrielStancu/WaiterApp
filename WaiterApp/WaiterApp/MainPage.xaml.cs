using Core.Helpers;
using Core.Models;
using Infrastructure.Helpers;
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
        private int _departmentId;

        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            _mainPageViewModel = mainPageViewModel;
            BindingContext = _mainPageViewModel;

            CurrentPageChanged += OnMainPageCurrentPageChanged;
            _departmentId = int.Parse(new ParametersLoader().GetParameter("departmentId"));
            LoadOrdersOnTimer();
            LoadTables();
            LoadProducts();
        }

        private void OnMainPageCurrentPageChanged(object sender, EventArgs e)
        {
            var tabbedPage = (TabbedPage)sender;
            var title = tabbedPage.CurrentPage.Title;

            if(title == "Orders")
            {
                LoadOrdersOnTimer();
            }
            else if (title == "Tables")
            {
                LoadTables();
            }
        }

        private async void LoadOrdersOnTimer()
        {
            await LoadOrders();
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                // Do something
                
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var page = (TabbedPage)this;

                    if(page.CurrentPage.Title == "Orders")
                    {
                        await LoadOrders();
                    }
                });
                return true; // True = Repeat again, False = Stop the timer
            });
        }

        private async Task LoadOrders()
        {
            var waiterId = int.Parse(new ParametersLoader().GetParameter("waiterId"));
            await _mainPageViewModel.LoadOrdersForWaiterAsync(waiterId);
        }

        public async void LoadTables()
        {
            var tables = await _mainPageViewModel.LoadTables(_departmentId);
            TablesLayout.Children.Clear();

            foreach (var table in tables)
            {
                var tableMessage = (table.Total == 0) ? string.Empty : table.Total.ToString();
                var tableButton = new Button()
                {
                    BackgroundColor = table.Color,
                    Text = $"{table.WaiterName ?? string.Empty}\n[{table.TableNumber}]{tableMessage}",
                    CornerRadius = 6,
                    HeightRequest = table.LengthY,
                    WidthRequest = table.LengthX,
                    BorderWidth = 2,
                    BorderColor = Color.Black,
                    ClassId = table.TableNumber.ToString()
                };
                tableButton.Clicked += OnTableButtonClicked;

                TablesLayout.Children.Add(tableButton, new Point(table.StartX, table.StartY));
            }
        }

        private async void OnTableButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            int tableNumber = int.Parse(button.ClassId);
            bool enabledTable = button.BackgroundColor != Color.Red;

            if(enabledTable)
            {
                _mainPageViewModel.SelectedTable =
                    _mainPageViewModel.Tables.FirstOrDefault(t => t.TableNumber == tableNumber);

                if(_mainPageViewModel.SelectedTable.Status == TableStatus.Free)
                {
                    CurrentPage = ProductsPage;
                }
                else if (_mainPageViewModel.SelectedTable.Status == TableStatus.TakenByCurrentWaiter)
                {
                    CurrentPage = OrderedProductsPage;
                    await _mainPageViewModel.LoadTableOrderedProducts();
                }
                
            }
        }

        public async void LoadProducts()
        {
            await _mainPageViewModel.LoadProductsAsync(_departmentId);
        }

        private void OnGroupSelectedItemChanged(object sender, EventArgs e)
        {
            _mainPageViewModel.FilterSubgroups();
            _mainPageViewModel.FilterProducts();
        }

        private void OnSubgroupSelectedIndexChanged(object sender, EventArgs e)
        {
            _mainPageViewModel.FilterProducts();
        }

        private void OnProductNameTextChanged(object sender, TextChangedEventArgs e)
        {
            _mainPageViewModel.FilterProducts();
        }

        private void OnProductSequenceTextChanged(object sender, TextChangedEventArgs e)
        {
            _mainPageViewModel.FilterProducts();
        }

        private async void OnOrderProductQuantityTextChanged(object sender, TextChangedEventArgs e)
        {
            var orderProduct = (OrderProduct)((Entry)sender).BindingContext;

            if (orderProduct != null)
            {
                bool parsed = double.TryParse(((Entry)sender).Text, out double _);

                if (!parsed && !(((Entry)sender).Text == string.Empty))
                {
                    await DisplayAlert("Error", "Not a valid quantity!", "OK");
                }
                else if (!(((Entry)sender).Text == string.Empty))
                {
                    await _mainPageViewModel.UpdateProductQuantity(orderProduct);
                }
                
            }
        }

        private async void OnDeleteOrderProductClicked(object sender, EventArgs e)
        {
            var orderProduct = (OrderProduct)((Button)sender).BindingContext;

            if (orderProduct != null)
            {
                await _mainPageViewModel.DeleteProduct(orderProduct);
            }
        }
    }
}