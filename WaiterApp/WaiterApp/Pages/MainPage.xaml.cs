using Core.Business;
using Core.Models;
using Infrastructure.Business;
using Infrastructure.Business.Factories;
using Infrastructure.Business.Parameters;
using Infrastructure.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        private readonly IMainPageViewModel _mainPageViewModel;
        private readonly ITableButtonFactory _tableButtonFactory;
        private readonly IProductsDrawer _productsDrawer;
        private readonly int _departmentId;
        private Page _lastPage;

        public MainPage(
            IMainPageViewModel mainPageViewModel,
            ITableButtonFactory tableButtonFactory,
            IProductsDrawer productsDrawer)
        {
            InitializeComponent();
            _lastPage = OrdersPage;
            _mainPageViewModel = mainPageViewModel;
            _tableButtonFactory = tableButtonFactory;
            _productsDrawer = productsDrawer;
            BindingContext = _mainPageViewModel;

            CurrentPageChanged += OnMainPageCurrentPageChanged;
            _departmentId = int.Parse(ParametersLoader.Parameters[AppParameters.DepartmentId]);
            LoadOrdersOnTimer();
            LoadTables();
            LoadProducts();
        }

        private void LoadProducts()
        {
            _mainPageViewModel.LoadProducts(_departmentId);
            DrawProducts();
        }

        private void DrawProducts()
        {
            _productsDrawer.Draw(ProductsGrid, _mainPageViewModel.Products);

            foreach (var child in ProductsGrid.Children)
            {
                (child as Button).Clicked += OnProductButtonClicked;
            }
        }

        private void OnMainPageCurrentPageChanged(object sender, EventArgs e)
        {
            if (CurrentPage == OrdersPage)
            {
                _lastPage = OrdersPage;
                _mainPageViewModel.SelectedTable = null;
                LoadOrdersOnTimer();
            }
            else if (CurrentPage == TablesPage)
            {
                _lastPage = TablesPage;
                _mainPageViewModel.SelectedTable = null;
                LoadTables();
            }
            else if (CurrentPage == ProductsPage && _lastPage == OrderedProductsPage)
            {
                _lastPage = ProductsPage;
            }
            else if (CurrentPage == OrderedProductsPage && _lastPage == ProductsPage)
            {
                _lastPage = OrderedProductsPage;
            }
            else
            {
                if(_mainPageViewModel.SelectedTable == null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CurrentPage = _lastPage;
                    });
                }
            }
        }

        private void LoadOrdersOnTimer()
        {
            LoadOrders();
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                // Do something
                // TODO: replace 10 with parameter
                
                Device.BeginInvokeOnMainThread(() =>
                {
                    var page = (TabbedPage)this;

                    if(page.CurrentPage.Title == "Orders")
                    {
                        LoadOrders();
                    }
                });
                return true; // True = Repeat again, False = Stop the timer
            });
        }

        private void LoadOrders()
        {
            var waiterId = int.Parse(ParametersLoader.Parameters[AppParameters.WaiterId]);
            _mainPageViewModel.LoadOrdersForWaiter(waiterId);
        }

        private void LoadTables()
        {
            var tables = _mainPageViewModel.LoadTables(_departmentId);
            TablesLayout.Children.Clear();

            foreach (var table in tables)
            {
                var tableButton = _tableButtonFactory.Build(table);
                tableButton.Clicked += OnTableButtonClicked;
                TablesLayout.Children.Add(tableButton, new Point(table.StartX, table.StartY));
            }
        }

        private void OnTableButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            int tableNumber = int.Parse(button.ClassId);
            bool enabledTable = button.BackgroundColor != Color.Red;

            if(enabledTable)
            {
                _mainPageViewModel.SelectedTable =
                    _mainPageViewModel.Tables.FirstOrDefault(t => t.TableNumber == tableNumber);

                if(_mainPageViewModel.SelectedTable.Status == TableStatus.Free)
                {
                    CurrentPage = ProductsPage;
                    _mainPageViewModel.ClearTable();
                }
                else if (_mainPageViewModel.SelectedTable.Status == TableStatus.TakenByCurrentWaiter)
                {
                    CurrentPage = OrderedProductsPage;
                    _mainPageViewModel.LoadTableOrderedProducts();
                }   
            }
        }

        private void OnProductButtonClicked(object sender, EventArgs e)
        {
            var product = (sender as Button).BindingContext as Product;
            _mainPageViewModel.AddProduct(product);
        }

        private void OnGroupSelectedItemChanged(object sender, EventArgs e)
        {
            _mainPageViewModel.FilterSubgroups();
            _mainPageViewModel.FilterProducts();
            DrawProducts();
        }

        private void OnSubgroupSelectedIndexChanged(object sender, EventArgs e)
        {
            _mainPageViewModel.FilterProducts();
            DrawProducts();
        }

        private void OnProductNameTextChanged(object sender, TextChangedEventArgs e)
        {
            _mainPageViewModel.FilterProducts();
            DrawProducts();
        }

        private void OnProductSequenceTextChanged(object sender, TextChangedEventArgs e)
        {
            _mainPageViewModel.FilterProducts();
            DrawProducts();
        }

        private async void OnOrderProductQuantityTextChanged(object sender, TextChangedEventArgs e)
        {
            var orderProduct = (OrderProduct)((Entry)sender).BindingContext;

            if (orderProduct != null)
            {
                bool parsed = double.TryParse(((Entry)sender).Text, out double _);

                if (!parsed && !(((Entry)sender).Text == string.Empty))
                {
                    await DisplayAlert("Error", "Not a valid quantity.", "OK");
                }
                else if (!(((Entry)sender).Text == string.Empty))
                {
                    _mainPageViewModel.UpdateProductQuantity(orderProduct);
                }
                
            }
        }

        private void OnDeleteOrderProductClicked(object sender, EventArgs e)
        {
            var orderProduct = (OrderProduct)((Button)sender).BindingContext;

            if (orderProduct != null)
            {
                _mainPageViewModel.DeleteProduct(orderProduct);
            }
        }
    }
}