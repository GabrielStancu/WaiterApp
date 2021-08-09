using Core.Helpers;
using Core.Models;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task LoadOrdersForWaiterAsync(int waiterId)
        {
            var orderProducts = await _orderProductRepository.LoadOrdersForWaiterAsync(waiterId);

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

        public async Task<IEnumerable<DrawnTable>> LoadTables(int departmentId)
        {
            Tables = await _tableRepository.GetTablesForDepartmentAsync(departmentId) as List<Table>;
            Orders = await _orderRepository.LoadOrdersForDepartmentAsync(departmentId) as List<Order>;
            int waiterId = int.Parse(new ParametersLoader().GetParameter("waiterId"));
            var tableDrawer = new TableDrawer();

            return tableDrawer.DrawTables(Tables, Orders, waiterId, 461, 744);
        }

        public async Task LoadProductsAsync(int departmentId)
        {
            _unfilteredGroups = (List<Group>)await _groupRepository.GetGroupsByDepartmentAsync(departmentId);
            _unfilteredSubgroups = (List<Subgroup>)await _subgroupRepository.GetSubgroupsByDepartmentAsync(departmentId);
            _unfilteredProducts = (List<Product>)await _productRepository.GetProductsByDepartmentAsync(departmentId);

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

        public async Task LoadTableOrderedProducts()
        {
            CurrentOrder = Orders.FirstOrDefault(o => o.TableId == SelectedTable.Id && o.Paid == false);
            var orderedProducts = await _orderProductRepository.LoadOrdersForTableAsync(SelectedTable.Id);

            TableOrderedProducts.Clear();
            foreach (var orderedProduct in orderedProducts)
            {
                TableOrderedProducts.Add(orderedProduct);
            }
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

            foreach (var subgroup in _unfilteredSubgroups)
            {
                if (subgroup.GroupId == SelectedGroup.Id || SelectedGroup.Id == 0)
                {
                    Subgroups.Add(subgroup);
                }
            }
        }

        public void FilterProducts()
        {
            //to use Specification DP here...

            Products.Clear();
            if(SelectedGroup != null)
            {
                if(SelectedSubgroup != null)
                {
                    //Group and subgroup selected
                    foreach (var product in _unfilteredProducts)
                    {
                        if((product.GroupId == SelectedGroup.Id ||SelectedGroup.Id == 0)
                            && (product.SubgroupId == SelectedSubgroup.Id || SelectedSubgroup.Id == 0))
                        {
                            if(product.Name.ToUpper().Contains(ProductName.ToUpper()) 
                                && product.Sequence.ToUpper().Contains(ProductSequence.ToUpper()))
                            {
                                Products.Add(product);
                            }
                        }
                    }
                }
                else
                {
                    //just Group selected
                    foreach (var product in _unfilteredProducts)
                    {
                        if (product.GroupId == SelectedGroup.Id || SelectedGroup.Id == 0)
                        {
                            if (product.Name.ToUpper().Contains(ProductName.ToUpper())
                                && product.Sequence.ToUpper().Contains(ProductSequence.ToUpper()))
                            {
                                Products.Add(product);
                            }
                        }
                    }
                }
            }
            else if(SelectedSubgroup != null)
            {
                //just Subgroup selected
                foreach (var product in _unfilteredProducts)
                {
                    if (product.SubgroupId == SelectedSubgroup.Id || SelectedSubgroup.Id == 0)
                    {
                        if (product.Name.ToUpper().Contains(ProductName.ToUpper())
                                && product.Sequence.ToUpper().Contains(ProductSequence.ToUpper()))
                        {
                            Products.Add(product);
                        }
                    }
                }
            }
            else
            {
                //neither Group nor Subgroup
                foreach (var product in _unfilteredProducts)
                {
                    if (product.Name.ToUpper().Contains(ProductName.ToUpper())
                                && product.Sequence.ToUpper().Contains(ProductSequence.ToUpper()))
                    {
                        Products.Add(product);
                    }
                }
            }
        }

        public async Task AddProduct(Product p)
        {
            CurrentOrder = Orders.FirstOrDefault(o => o.TableId == SelectedTable.Id && o.Paid == false);

            if (CurrentOrder is null)
            {
                await CreateNewOrder();
                await SetTableStatusTaken();
            }

            await AddOrderProduct(p);
        }
        
        public async Task UpdateProductQuantity(OrderProduct orderProduct)
        {
            ComputeOrderTotal();
            //update db
            await _productRepository.UpdateAsync(orderProduct.Product); //update the stock
            if(orderProduct.Id != 0)
            {
                await _orderProductRepository.UpdateAsync(orderProduct); //update the order quantity
            }
            await _orderRepository.UpdateAsync(CurrentOrder); //update the order total
        }
        public async Task DeleteProduct(OrderProduct orderProduct)
        {
            TableOrderedProducts.Remove(orderProduct);
            ComputeOrderTotal();

            //update db
            await _orderProductRepository.DeleteAsync(orderProduct);
            await _orderRepository.UpdateAsync(CurrentOrder);

            if (TableOrderedProducts.Count == 0)
            {
                SetTableStatusOnEmpty();
                await _tableRepository.UpdateAsync(SelectedTable);

                Orders.Remove(CurrentOrder);
                await _orderRepository.DeleteAsync(CurrentOrder);
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

        private async Task CreateNewOrder()
        {
            CurrentOrder = new Order()
            {
                WaiterId = int.Parse(new ParametersLoader().GetParameter("waiterId")),
                TableId = SelectedTable.Id
            };
            Orders.Add(CurrentOrder);
            await _orderRepository.InsertAsync(CurrentOrder);
        }

        private async Task SetTableStatusTaken()
        {
            SelectedTable.WaiterId = int.Parse(new ParametersLoader().GetParameter("waiterId"));
            SelectedTable.Status = TableStatus.TakenByCurrentWaiter;
            await _tableRepository.UpdateAsync(SelectedTable);
        }

        private async Task AddOrderProduct(Product p)
        {
            var orderProduct = TableOrderedProducts.FirstOrDefault(op => op.Product.Sequence == p.Sequence);
            if (orderProduct is null)
            {
                await CreateNewOrderProduct(p);
                await LoadTableOrderedProducts();
            }
            else
            {
                orderProduct.Quantity++;
                ComputeOrderTotal();
                await UpdateProductQuantity(orderProduct);
            }
        }

        private async Task CreateNewOrderProduct(Product p)
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
            await _orderProductRepository.RegisterNewOrderProductAsync(orderedProduct);

            //update order total
            ComputeOrderTotal();
            await _orderRepository.UpdateAsync(CurrentOrder);
        }
    }
}
