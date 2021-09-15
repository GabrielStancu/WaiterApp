using Core.Business;
using Core.Models;
using Infrastructure.Business.Filter;
using Infrastructure.Business.Parameters;
using Infrastructure.Business.TablesDrawing;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Infrastructure.ViewModels
{
    public class MainPageViewModel: BaseViewModel
    {
        public ObservableCollection<Group> Groups { get; set; }
            = new ObservableCollection<Group>();
        public ObservableCollection<Subgroup> Subgroups { get; set; }
            = new ObservableCollection<Subgroup>();
        public ObservableCollection<Product> Products { get; set; }
            = new ObservableCollection<Product>();
        public ObservableCollection<OrderProduct> WaiterOrderedProducts { get; set; }
            = new ObservableCollection<OrderProduct>();
        public ObservableCollection<OrderProduct> TableOrderedProducts { get; set; }
            = new ObservableCollection<OrderProduct>();
        public Group SelectedGroup { get; set; }
        public Subgroup SelectedSubgroup { get; set; }
        public Table SelectedTable { get; set; }
        public Order CurrentOrder { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSequence { get; set; } = string.Empty;
        public List<Table> Tables { get; set; } = new List<Table>();
        public List<Order> Orders { get; set; } = new List<Order>();

        private readonly OrderProductRepository _orderProductRepository;
        private readonly GroupRepository _groupRepository;
        private readonly SubgroupRepository _subgroupRepository;
        private readonly ProductRepository _productRepository;
        private readonly TableRepository _tableRepository;
        private readonly OrderRepository _orderRepository;

        private List<Product> _unfilteredProducts = new List<Product>();
        private List<Group> _unfilteredGroups = new List<Group>();
        private List<Subgroup> _unfilteredSubgroups = new List<Subgroup>();

        public MainPageViewModel(
            OrderProductRepository orderProductRepository,
            GroupRepository groupRepository,
            SubgroupRepository subgroupRepository,
            ProductRepository productRepository,
            TableRepository tableRepository,
            OrderRepository orderRepository)
        {
            _orderProductRepository = orderProductRepository;
            _groupRepository = groupRepository;
            _subgroupRepository = subgroupRepository;
            _productRepository = productRepository;
            _tableRepository = tableRepository;
            _orderRepository = orderRepository;
        }

        public void LoadOrdersForWaiter(int waiterId)
        {
            var orderProducts = _orderProductRepository.LoadOrdersForWaiter(waiterId);

            WaiterOrderedProducts.Clear();
            foreach (var orderProduct in orderProducts)
            {
                if(orderProduct.ServingTime.HasValue)
                {
                    orderProduct.Color = Color.Green;
                }
                WaiterOrderedProducts.Add(orderProduct);
            }
        } 

        public IEnumerable<DrawnTable> LoadTables(int departmentId)
        {
            Tables = _tableRepository.GetTablesForDepartment(departmentId) as List<Table>;
            Orders = _orderRepository.LoadOrdersForDepartment(departmentId) as List<Order>;
            int waiterId = int.Parse(ParametersLoader.Parameters[AppParameters.WaiterId]);
            var tableDrawer = new TableDrawer();

            return tableDrawer.DrawTables(Tables, Orders, waiterId, 461, 744);
        }

        public void LoadProducts(int departmentId)
        {
            _unfilteredGroups = (List<Group>) _groupRepository.GetGroupsByDepartment(departmentId);
            _unfilteredSubgroups = (List<Subgroup>) _subgroupRepository.GetSubgroupsByDepartment(departmentId);
            _unfilteredProducts = (List<Product>) _productRepository.GetProductsByDepartment(departmentId);

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

            var sf = new SubgroupFilter();
            sf.Filter(_unfilteredSubgroups, SelectedGroup).ToList()
                .ForEach(s => Subgroups.Add(s));
        }

        public void FilterProducts()
        {
            Products.Clear();

            var pf = new ProductsFilter();
            pf.Filter(_unfilteredProducts, SelectedGroup, SelectedSubgroup, ProductName, ProductSequence).ToList()
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
            if(orderProduct.Id != 0)
            {
                _orderProductRepository.Update(orderProduct); //update the order quantity
            }
            _orderRepository.Update(CurrentOrder); //update the order total
        }
        public void DeleteProduct(OrderProduct orderProduct)
        {
            TableOrderedProducts.Remove(orderProduct);
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
        }
        private void SetTableStatusOnEmpty()
        {
            SelectedTable.WaiterId = 0;
            SelectedTable.Waiter = null;
            SelectedTable.Status = TableStatus.Free;
        }

        private void CreateNewOrder()
        {
            CurrentOrder = new Order()
            {
                WaiterId = int.Parse(ParametersLoader.Parameters[AppParameters.WaiterId]),
                TableId = SelectedTable.Id
            };
            Orders.Add(CurrentOrder);
            _orderRepository.Insert(CurrentOrder);
        }

        private void SetTableStatusTaken()
        {
            SelectedTable.WaiterId = int.Parse(ParametersLoader.Parameters[AppParameters.WaiterId]);
            SelectedTable.Status = TableStatus.TakenByCurrentWaiter;
            _tableRepository.Update(SelectedTable);
        }

        private void AddOrderProduct(Product p)
        {
            var orderProduct = TableOrderedProducts.FirstOrDefault(op => op.Product.Sequence == p.Sequence);
            if (orderProduct is null)
            {
                CreateNewOrderProduct(p);
                LoadTableOrderedProducts();
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
