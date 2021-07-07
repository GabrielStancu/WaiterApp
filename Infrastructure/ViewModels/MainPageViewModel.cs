using Core.Models;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class MainPageViewModel: BaseViewModel
    {
        public ObservableCollection<OrderProduct> OrderProducts { get; set; }
            = new ObservableCollection<OrderProduct>();
        public ObservableCollection<Group> Groups { get; set; }
            = new ObservableCollection<Group>();
        public ObservableCollection<Subgroup> Subgroups { get; set; }
            = new ObservableCollection<Subgroup>();
        public ObservableCollection<Product> Products { get; set; }
            = new ObservableCollection<Product>();
        public Group SelectedGroup { get; set; }
        public Subgroup SelectedSubgroup { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSequence { get; set; } = string.Empty;

        //SelectedTable ???

        private readonly OrderProductRepository _orderProductRepository;
        private readonly GroupRepository _groupRepository;
        private readonly SubgroupRepository _subgroupRepository;
        private readonly ProductRepository _productRepository;
        private readonly TableRepository _tableRepository;

        private List<Product> _unfilteredProducts = new List<Product>();
        private List<Group> _unfilteredGroups = new List<Group>();
        private List<Subgroup> _unfilteredSubgroups = new List<Subgroup>();

        public MainPageViewModel(
            OrderProductRepository orderProductRepository,
            GroupRepository groupRepository,
            SubgroupRepository subgroupRepository,
            ProductRepository productRepository,
            TableRepository tableRepository)
        {
            _orderProductRepository = orderProductRepository;
            _groupRepository = groupRepository;
            _subgroupRepository = subgroupRepository;
            _productRepository = productRepository;
            _tableRepository = tableRepository;
        }

        public async Task LoadOrdersForWaiterAsync(int waiterId)
        {
            var orders = await _orderProductRepository.LoadOrdersForWaiterAsync(waiterId);

            OrderProducts.Clear();
            foreach (var order in orders)
            {
                OrderProducts.Add(order);
            }
        }

        public async Task<IEnumerable<DrawnTable>> LoadTables(int departmentId)
        {
            var tables = await _tableRepository.GetTablesForDepartmentAsync(departmentId);
            var tableDrawer = new TableDrawer();

            return tableDrawer.DrawTables(tables, 461, 744);
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
    }
}
