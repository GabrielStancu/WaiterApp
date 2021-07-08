using Core.Models;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public string ProductName { get; set; } = string.Empty;
        public string ProductSequence { get; set; } = string.Empty;
        public IEnumerable<Table> Tables { get; set; } = new List<Table>();
        public IEnumerable<Order> Orders { get; set; } = new List<Order>();
        public ICommand DeleteProductCommand { get; set; }
        public ICommand AddProductCommand { get; set; }

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
            DeleteProductCommand = new Command<Product>(async p => await DeleteProduct(p));
            AddProductCommand = new Command<Product>(async p => await AddProduct(p));
        }

        public async Task LoadOrdersForWaiterAsync(int waiterId)
        {
            var orderProducts = await _orderProductRepository.LoadOrdersForWaiterAsync(waiterId);

            WaiterOrderedProducts.Clear();
            foreach (var orderProduct in orderProducts)
            {
                WaiterOrderedProducts.Add(orderProduct);
            }
        } 

        public async Task<IEnumerable<DrawnTable>> LoadTables(int departmentId)
        {
            Tables = await _tableRepository.GetTablesForDepartmentAsync(departmentId);
            Orders = await _orderRepository.LoadOrdersForDepartmentAsync(departmentId);
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
            foreach (var group in _unfilteredGroups)
            {
                Groups.Add(group);
            }

            Subgroups.Clear();
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

            foreach (var subgroup in _unfilteredSubgroups)
            {
                if(subgroup.GroupId == SelectedGroup.Id)
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
                        if(product.GroupId == SelectedGroup.Id && product.SubgroupId == SelectedSubgroup.Id)
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
                        if (product.GroupId == SelectedGroup.Id)
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
                    if (product.SubgroupId == SelectedSubgroup.Id)
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
                //nor Group neither Subgroup
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

        private async Task AddProduct(Product p)
        {
            var order = Orders.FirstOrDefault(o => o.TableId == SelectedTable.Id);
            var orderedProduct = new OrderProduct()
            {
                Product = p,
                ProductId = p.Id,
                Quantity = 1,
                PlacementTime = DateTime.Now,
                Order = order,
                OrderId = order.Id
            };
            TableOrderedProducts.Add(orderedProduct);

            //add it to db
            await _orderProductRepository.RegisterNewOrderProductAsync(orderedProduct);
        }

        private async Task DeleteProduct(Product p)
        {
            string productName = p.Name;
            await Task.Delay(100);
        }
    }
}
