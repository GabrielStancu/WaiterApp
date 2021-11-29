using Core.Business;
using Core.Models;
using Infrastructure.Business;
using Infrastructure.Business.Factories;
using Infrastructure.Business.Parameters;
using Infrastructure.Business.Wifi;
using Infrastructure.Exceptions;
using Infrastructure.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        private readonly MainPageViewModel _mainPageViewModel;
        private readonly ITableButtonFactory _tableButtonFactory;
        private readonly IProductsDrawer _productsDrawer;
        private readonly IWifiConnectionChecker _wifiConnectionChecker;
        private readonly IWifiConnectionResponseParser _wifiConnectionResponseParser;
        private Page _lastPage;
        private bool _loadedPage = false;
        private bool _refreshedPage = false;

        public MainPage(
            MainPageViewModel mainPageViewModel,
            ITableButtonFactory tableButtonFactory,
            IProductsDrawer productsDrawer,
            IWifiConnectionChecker wifiConnectionChecker,
            IWifiConnectionResponseParser wifiConnectionResponseParser)
        {
            InitializeComponent();
            _lastPage = OrdersPage;
            _mainPageViewModel = mainPageViewModel;
            _tableButtonFactory = tableButtonFactory;
            _productsDrawer = productsDrawer;
            _wifiConnectionChecker = wifiConnectionChecker;
            _wifiConnectionResponseParser = wifiConnectionResponseParser;
            BindingContext = _mainPageViewModel;

            CurrentPageChanged += OnMainPageCurrentPageChanged;

            var loadOrdersOnTimer = LoadOrdersOnTimer();
            var loadTables = LoadTables();
            var loadProducts =  LoadProducts();
            var loadAllOrders =  LoadAllOrders();

            Task.WaitAll(loadOrdersOnTimer, loadTables, loadProducts, loadAllOrders);          
            _loadedPage = true;
        }

        private async Task LoadProducts()
        {
            try
            {
                _mainPageViewModel.LoadProducts();
                DrawProducts();
            }
            catch(WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void DrawProducts()
        {
            _productsDrawer.Draw(ProductsGrid, _mainPageViewModel.Products);

            foreach (var child in ProductsGrid.Children)
            {
                (child as Button).Clicked += OnProductButtonClicked;
            }
        }

        private async void OnMainPageCurrentPageChanged(object sender, EventArgs e)
        {
            if (_refreshedPage)
            {
                _refreshedPage = false;
            }
            else if (CurrentPage == OrdersPage)
            {
                _lastPage = OrdersPage;
                _mainPageViewModel.SelectedTable = null;
                try
                {
                    await LoadOrdersOnTimer();
                }
                catch (WifiConnectionException ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
            else if (CurrentPage == TablesPage)
            {
                _lastPage = TablesPage;
                _mainPageViewModel.SelectedTable = null;
                try
                {
                    await LoadTables();
                }
                catch (WifiConnectionException ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                } 
            }
            else if (CurrentPage == OrderedProductsPage)
            {
                _lastPage = OrderedProductsPage;
                _mainPageViewModel.SelectedTable = null;
                try
                {
                    await LoadAllOrders();
                }
                catch (WifiConnectionException ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
                
                //var connection = _wifiConnectionChecker.CheckConnection();
                //string message = _wifiConnectionResponseParser.GenerateResponse(connection);
                //if (connection != WifiConnectionResponse.WIFI_DATA_INTERNET)
                //{
                //    await DisplayAlert("Error", message, "OK");
                //}
            }
            else
            {
                if(_mainPageViewModel.SelectedTable == null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _refreshedPage = true;
                        CurrentPage = _lastPage;
                    });
                }
            }
        }

        private async Task LoadOrdersOnTimer()
        {
            string timerRefreshParam = ParametersLoader.Parameters[AppParameters.ReadOrdersTimer];
            int timerRefresh = int.Parse(timerRefreshParam);
            try
            {
                LoadOrders();
            }
            catch (WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            

            Device.StartTimer(TimeSpan.FromSeconds(timerRefresh), () =>
            {                     
                var page = (TabbedPage)this;

                if(page.CurrentPage.Title == "Orders")
                {
                    LoadOrders();
                }

                return true; // True = Repeat again, False = Stop the timer
            });
        }

        private void LoadOrders()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    _mainPageViewModel.LoadOrdersForWaiter();
                }
                catch (WifiConnectionException ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            });
        }

        private async Task LoadAllOrders()
        {
            try
            {
                _mainPageViewModel.LoadAllOrdersForWaiter();
            }
            catch (WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task LoadTables()
        {
            try
            {
                var tables = _mainPageViewModel.LoadTables();
                TablesLayout.Children.Clear();

                foreach (var table in tables)
                {
                    var tableButton = _tableButtonFactory.Build(table);
                    tableButton.Clicked += OnTableButtonClicked;
                    TablesLayout.Children.Add(tableButton, new Point(table.StartX, table.StartY));
                }
            }
            catch (WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnTableButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            int tableNumber = int.Parse(button.ClassId);
            var currentTable = _mainPageViewModel.Tables.FirstOrDefault(t => t.TableNumber == tableNumber);
            var waiterId = int.Parse(ParametersLoader.Parameters[AppParameters.WaiterId]);
            bool enabledTable = currentTable.GetStatus(waiterId) != TableStatus.TakenByOtherWaiter;

            if(enabledTable)
            {
                _mainPageViewModel.SelectedTable =
                    _mainPageViewModel.Tables.FirstOrDefault(t => t.TableNumber == tableNumber);
                CurrentPage = ProductsPage;

                if(_mainPageViewModel.SelectedTable.GetStatus(waiterId) == TableStatus.Free)
                {
                    _mainPageViewModel.ClearTable();
                }
                else if (_mainPageViewModel.SelectedTable.GetStatus(waiterId) == TableStatus.TakenByCurrentWaiter)
                {
                    try
                    {
                        _mainPageViewModel.LoadTableOrderedProducts();
                    }
                    catch (WifiConnectionException ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                    }
                }   
            }
        }

        private async void OnProductButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var product = (sender as Button).BindingContext as Product;
                _mainPageViewModel.AddProduct(product);
            }
            catch (WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnGroupSelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                _mainPageViewModel.FilterSubgroups();
                _mainPageViewModel.FilterProducts();
                DrawProducts();
            }
            catch (WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            } 
        }

        private async void OnSubgroupSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _mainPageViewModel.FilterProducts();
                DrawProducts();
            }
            catch (WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnProductNameTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                _mainPageViewModel.FilterProducts();
                DrawProducts();
            }
            catch (WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnProductSequenceTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                _mainPageViewModel.FilterProducts();
                DrawProducts();
            }
            catch (WifiConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnOrderProductQuantityTextChanged(object sender, TextChangedEventArgs e)
        {
            var orderProduct = (OrderProduct)((Entry)sender).BindingContext;

            if (orderProduct != null && _loadedPage)
            {
                bool parsed = double.TryParse(((Entry)sender).Text, out double _);

                if (!parsed && !(((Entry)sender).Text == string.Empty))
                {
                    await DisplayAlert("Error", "Not a valid quantity.", "OK");
                }
                else if (!(((Entry)sender).Text == string.Empty))
                {
                    try
                    {
                        _mainPageViewModel.UpdateProductQuantity(orderProduct);
                    }
                    catch (WifiConnectionException ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                    }
                }
                
            }
        }

        private async void OnDeleteOrderProductClicked(object sender, EventArgs e)
        {
            var orderProduct = (OrderProduct)((Button)sender).BindingContext;

            if (orderProduct != null)
            {
                try
                {
                    _mainPageViewModel.DeleteProduct(orderProduct);
                }
                catch (WifiConnectionException ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }
    }
}