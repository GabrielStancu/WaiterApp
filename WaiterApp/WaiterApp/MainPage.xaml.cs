using Core.Helpers;
using Core.Models;
using Infrastructure.Helpers;
using Infrastructure.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        private readonly MainPageViewModel _mainPageViewModel;
        private readonly int _departmentId;
        private Page _lastPage;

        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            _lastPage = OrdersPage;
            _mainPageViewModel = mainPageViewModel;
            BindingContext = _mainPageViewModel;

            CurrentPageChanged += OnMainPageCurrentPageChanged;
            _departmentId = int.Parse(ParametersLoader.Parameters["departmentId"]);
            LoadOrdersOnTimer();
            LoadTables();
            LoadProducts();
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
            var waiterId = int.Parse(ParametersLoader.Parameters["waiterId"]);
            _mainPageViewModel.LoadOrdersForWaiter(waiterId);
        }

        public void LoadTables()
        {
            var tables = _mainPageViewModel.LoadTables(_departmentId);
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

        private void OnTableButtonClicked(object sender, EventArgs e)
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
                    _mainPageViewModel.ClearTable();
                }
                else if (_mainPageViewModel.SelectedTable.Status == TableStatus.TakenByCurrentWaiter)
                {
                    CurrentPage = OrderedProductsPage;
                    _mainPageViewModel.LoadTableOrderedProducts();
                }
                
            }
        }

        public void LoadProducts()
        {
            _mainPageViewModel.LoadProducts(_departmentId);
            DrawProducts();
        }

        private void DrawProducts()
        {
            ProductsGrid.Children?.Clear();

            int productsCount = _mainPageViewModel.Products.Count;
            int productsPerRow = Int32.Parse(ParametersLoader.Parameters["buttonsPerLine"]);
            int rows = productsCount / productsPerRow;
            int crtRow = 0, crtCol = 0;
            if(productsCount % productsPerRow != 0)
            {
                rows++;
            }


            ProductsGrid.RowDefinitions = new RowDefinitionCollection();
            ProductsGrid.ColumnDefinitions = new ColumnDefinitionCollection();

            for(int i = 0; i<productsPerRow; i++)
            {
                ProductsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for(int i = 0; i<rows; i++)
            {
                ProductsGrid.RowDefinitions.Add(new RowDefinition());
            }

            for(int i = 0; i< productsCount; i++)
            {
                var productBtn = new Button
                {
                    Text = _mainPageViewModel.Products[i].Name,
                    BackgroundColor = Color.DarkGray,
                    TextColor = Color.White,
                    CornerRadius = 20,
                    BindingContext = _mainPageViewModel.Products[i],
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HeightRequest = 100
                };

                productBtn.Clicked += OnProductButtonClicked;
                ProductsGrid.Children.Add(productBtn, crtCol++, crtRow);
                if(crtCol == productsPerRow)
                {
                    crtCol = 0;
                    crtRow++;
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
                    await DisplayAlert("Error", "Not a valid quantity!", "OK");
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