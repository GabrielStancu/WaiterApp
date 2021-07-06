using Core.Models;
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

        private readonly OrderProductRepository _orderProductRepository;
        private readonly GroupRepository _groupRepository;
        private readonly SubgroupRepository _subgroupRepository;
        private readonly ProductRepository _productRepository;

        public MainPageViewModel(
            OrderProductRepository orderProductRepository,
            GroupRepository groupRepository,
            SubgroupRepository subgroupRepository,
            ProductRepository productRepository)
        {
            _orderProductRepository = orderProductRepository;
            _groupRepository = groupRepository;
            _subgroupRepository = subgroupRepository;
            _productRepository = productRepository;
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

        public async Task LoadTables(int departmentId)
        {
            //TODO
        }

        public async Task LoadProductsAsync(int departmentId)
        {
            var groups = await _groupRepository.GetGroupsByDepartmentAsync(departmentId);
            var subgroups = await _subgroupRepository.GetSubgroupsByDepartmentAsync(departmentId);
            var products = await _productRepository.GetProductsByDepartmentAsync(departmentId);

            Groups.Clear();
            foreach (var group in groups)
            {
                Groups.Add(group);
            }

            Subgroups.Clear();
            foreach (var subgroup in subgroups)
            {
                Subgroups.Add(subgroup);
            }

            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }
    }
}
