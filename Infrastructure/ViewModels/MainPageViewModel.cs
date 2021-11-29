using Core.Models;
using Infrastructure.Business.ControlsDrawing;
using Infrastructure.Business.Filter;
using Infrastructure.Business.Parameters;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Infrastructure.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<Group> Groups { get; set; }
            = new ObservableCollection<Group>();
        public ObservableCollection<Subgroup> Subgroups { get; set; }
            = new ObservableCollection<Subgroup>();
        public ObservableCollection<Product> Products { get; set; }
            = new ObservableCollection<Product>();
        public ObservableCollection<OrderProduct> WaiterOrderedProductsRecipes { get; set; }
            = new ObservableCollection<OrderProduct>();
        public ObservableCollection<OrderProduct> TableOrderedProducts { get; set; }
            = new ObservableCollection<OrderProduct>();
        public ObservableCollection<OrderProduct> WaiterOrderedProducts { get; set; }
            = new ObservableCollection<OrderProduct>();
        public Group SelectedGroup { get; set; }
        public Subgroup SelectedSubgroup { get; set; }
        public Table SelectedTable { get; set; }
        public Order CurrentOrder { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSequence { get; set; } = string.Empty;
        public List<Table> Tables { get; set; } = new List<Table>();
        public List<Order> Orders { get; set; } = new List<Order>();
        private double _currentTableTotal;
        public double CurrentTableTotal
        {
            get => _currentTableTotal;
            set
            {
                _currentTableTotal = value;
                SetProperty<double>(ref _currentTableTotal, value);
            }
        }

        private int _waiterId;
        public int WaiterId
        {
            get => _waiterId;
            set
            {
                _waiterId = value;
                SetProperty<int>(ref _waiterId, value);
            }
        }

        private int _departmentId;
        public int DepartmentId
        {
            get => _departmentId;
            set
            {
                _departmentId = value;
                SetProperty<int>(ref _departmentId, value);
            }
        }

        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISubgroupRepository _subgroupRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ITableDrawer _tableDrawer;
        private readonly IProductsFilter _productsFilter;
        private readonly ISubgroupsFilter _subgroupsFilter;
        private List<Product> _unfilteredProducts = new List<Product>();
        private List<Group> _unfilteredGroups = new List<Group>();
        private List<Subgroup> _unfilteredSubgroups = new List<Subgroup>();

        public MainPageViewModel(
            IOrderProductRepository orderProductRepository,
            IGroupRepository groupRepository,
            ISubgroupRepository subgroupRepository,
            IProductRepository productRepository,
            ITableRepository tableRepository,
            IOrderRepository orderRepository,
            ITableDrawer tableDrawer,
            IProductsFilter productsFilter,
            ISubgroupsFilter subgroupsFilter)
        {
            _orderProductRepository = orderProductRepository;
            _groupRepository = groupRepository;
            _subgroupRepository = subgroupRepository;
            _productRepository = productRepository;
            _tableRepository = tableRepository;
            _orderRepository = orderRepository;
            _tableDrawer = tableDrawer;
            _productsFilter = productsFilter;
            _subgroupsFilter = subgroupsFilter;

            WaiterId = int.Parse(ParametersLoader.Parameters[AppParameters.WaiterId]);
            DepartmentId = int.Parse(ParametersLoader.Parameters[AppParameters.DepartmentId]);
        }

        public void LoadOrdersForWaiter()
        {
            var orderProducts = _orderProductRepository.LoadOrdersForWaiter(WaiterId);

            WaiterOrderedProductsRecipes.Clear();
            foreach (var orderProduct in orderProducts)
            {
                if (orderProduct.ServingTime.HasValue)
                {
                    orderProduct.Color = Color.FromHex("#922636"); //brown
                    orderProduct.TextColor = Color.FromHex("#ffffff"); //white
                }
                else
                {
                    orderProduct.Color = Color.FromHex("#e0e0e0"); //gray
                    orderProduct.TextColor = Color.FromHex("#52000B"); // dark brown
                }
                WaiterOrderedProductsRecipes.Add(orderProduct);
            }
        }

        public void LoadAllOrdersForWaiter()
        {
            var orderProducts = _orderProductRepository.LoadAllOrdersForWaiter(WaiterId).ToList();

            WaiterOrderedProducts.Clear();
            orderProducts.ForEach(op => WaiterOrderedProducts.Add(op));
        }

        public IEnumerable<DrawnTable> LoadTables()
        {
            Tables = _tableRepository.GetTablesForDepartment(DepartmentId) as List<Table>;
            Orders = _orderRepository.LoadOrdersForDepartment(DepartmentId) as List<Order>;

            return _tableDrawer.DrawTables(Tables, Orders, WaiterId, 461, 744);
        }

        public void LoadProducts()
        {
            _unfilteredGroups = (List<Group>)_groupRepository.GetGroupsByDepartment(DepartmentId);
            _unfilteredSubgroups = (List<Subgroup>)_subgroupRepository.GetSubgroupsByDepartment(DepartmentId);
            _unfilteredProducts = (List<Product>)_productRepository.GetProductsByDepartment(DepartmentId);

            Groups.Clear();
            Groups.Add(new Group()
            {
                DepartmentId = 0,
                Name = "-"
            });
            foreach (var group in _unfilteredGroups)
            {
                Groups.Add(group);
            }

            Subgroups.Clear();
            Subgroups.Add(new Subgroup()
            {
                DepartmentId = 0,
                GroupId = 0,
                Name = "-"
            });
            foreach (var subgroup in _unfilteredSubgroups)
            {
                Subgroups.Add(subgroup);
            }

            Products.Clear();
            foreach (var product in _unfilteredProducts)
            {
                Products.Add(product);
            }
        }

        public void LoadTableOrderedProducts()
        {
            CurrentOrder = Orders.FirstOrDefault(o => o.TableId == SelectedTable.Id && o.Paid == false);
            var orderedProducts = _orderProductRepository.LoadOrdersForTable(SelectedTable.Id);

            TableOrderedProducts.Clear();
            foreach (var orderedProduct in orderedProducts)
            {
                TableOrderedProducts.Add(orderedProduct);
            }
        }

        private void LoadWaiterOrderedProducts()
        {
            var orderedProducts = _orderProductRepository.LoadAllOrdersForWaiter(WaiterId);

            WaiterOrderedProducts.Clear();
            foreach (var orderedProduct in orderedProducts)
            {
                WaiterOrderedProducts.Add(orderedProduct);
            }
        }

        public void ClearTable()
        {
            TableOrderedProducts.Clear();
        }

        public void FilterSubgroups()
        {
            Subgroups.Clear();
            Subgroups.Add(new Subgroup()
            {
                DepartmentId = 0,
                GroupId = 0,
                Name = "-"
            });

            _subgroupsFilter.Filter(_unfilteredSubgroups, SelectedGroup).ToList()
                .ForEach(s => Subgroups.Add(s));
        }

        public void FilterProducts()
        {
            Products.Clear();
            _productsFilter.Filter(_unfilteredProducts, SelectedGroup, SelectedSubgroup, ProductName, ProductSequence).ToList()
                        .ForEach(p => Products.Add(p));
        }

        public void AddProduct(Product p)
        {
            CurrentOrder = Orders.FirstOrDefault(o => o.TableId == SelectedTable.Id && o.Paid == false);

            if (CurrentOrder is null)
            {
                CreateNewOrder();
                SetTableStatusTaken();
            }

            AddOrderProduct(p);
        }

        public void UpdateProductQuantity(OrderProduct orderProduct)
        {
            ComputeOrderTotal();
            
            //update db
            _productRepository.Update(orderProduct.Product); //update the stock
            if (orderProduct.Id != 0)
            {
                _orderProductRepository.Update(orderProduct); //update the order quantity

                var tableOp = TableOrderedProducts.FirstOrDefault(op => op.Id == orderProduct.Id);
                var waiterOp = WaiterOrderedProducts.FirstOrDefault(op => op.Id == orderProduct.Id);

                if (tableOp != null)
                {
                    tableOp.Quantity = orderProduct.Quantity;
                }
                if (waiterOp != null)
                {
                    waiterOp.Quantity = orderProduct.Quantity;
                } 
            }
            _orderRepository.Update(CurrentOrder); //update the order total
        }
        public void DeleteProduct(OrderProduct orderProduct)
        {
            var tableOp = TableOrderedProducts.FirstOrDefault(op => op.Id == orderProduct.Id);
            var waiterOp = WaiterOrderedProducts.FirstOrDefault(op => op.Id == orderProduct.Id);

            if(tableOp != null)
            {
                TableOrderedProducts.Remove(tableOp);
            }
            if(waiterOp != null)
            {
                WaiterOrderedProducts.Remove(waiterOp);
            }
            ComputeOrderTotal();

            //update db
            _orderProductRepository.Delete(orderProduct);
            _orderRepository.Update(CurrentOrder);

            if (TableOrderedProducts.Count == 0) 
            {
                SetTableStatusOnEmpty();
                _tableRepository.Update(SelectedTable);

                Orders.Remove(CurrentOrder);
                _orderRepository.Delete(CurrentOrder);
                CurrentOrder = null;
            }
        }

        private void ComputeOrderTotal()
        {
            CurrentOrder.Total = 0;

            foreach (var orderProduct in TableOrderedProducts)
            {
                CurrentOrder.Total += orderProduct.Quantity * double.Parse(orderProduct.Product.Price.ToString());
            }
            CurrentTableTotal = CurrentOrder.Total;
        }
        private void SetTableStatusOnEmpty()
        {
            SelectedTable.WaiterId = null;
            SelectedTable.Waiter = null;
        }

        private void CreateNewOrder()
        {
            CurrentOrder = new Order()
            {
                WaiterId = WaiterId,
                TableId = SelectedTable.Id
            };
            Orders.Add(CurrentOrder);
            _orderRepository.Insert(CurrentOrder);
        }

        private void SetTableStatusTaken()
        {
            SelectedTable.WaiterId = WaiterId;
            _tableRepository.Update(SelectedTable);
        }

        private void AddOrderProduct(Product p)
        {
            var orderProduct = TableOrderedProducts.FirstOrDefault(op => op.Product.Sequence == p.Sequence);
            if (orderProduct is null)
            {
                CreateNewOrderProduct(p);
                LoadTableOrderedProducts();
                LoadWaiterOrderedProducts();
            }
            else
            {
                orderProduct.Quantity++;
                ComputeOrderTotal();
                UpdateProductQuantity(orderProduct);
            }
        }

        private void CreateNewOrderProduct(Product p)
        {
            //insert new product
            var orderedProduct = new OrderProduct()
            {
                Product = p,
                ProductId = p.Id,
                Quantity = 1,
                PlacementTime = DateTime.Now,
                Order = CurrentOrder,
                OrderId = CurrentOrder.Id
            };
            TableOrderedProducts.Add(orderedProduct);

            //add it to db
            _orderProductRepository.RegisterNewOrderProduct(orderedProduct);

            //update order total
            ComputeOrderTotal();
            _orderRepository.Update(CurrentOrder);
        }
    }
}
