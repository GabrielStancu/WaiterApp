using Core.Models;
using Infrastructure.Business.ControlsDrawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Infrastructure.ViewModels
{
    public interface IMainPageViewModel
    {
        Order CurrentOrder { get; set; }
        ObservableCollection<Group> Groups { get; set; }
        List<Order> Orders { get; set; }
        string ProductName { get; set; }
        ObservableCollection<Product> Products { get; set; }
        string ProductSequence { get; set; }
        Group SelectedGroup { get; set; }
        Subgroup SelectedSubgroup { get; set; }
        Table SelectedTable { get; set; }
        ObservableCollection<Subgroup> Subgroups { get; set; }
        ObservableCollection<OrderProduct> TableOrderedProducts { get; set; }
        List<Table> Tables { get; set; }
        ObservableCollection<OrderProduct> WaiterOrderedProducts { get; set; }

        void AddProduct(Product p);
        void ClearTable();
        void DeleteProduct(OrderProduct orderProduct);
        void FilterProducts();
        void FilterSubgroups();
        void LoadOrdersForWaiter(int waiterId);
        void LoadProducts(int departmentId);
        void LoadTableOrderedProducts();
        IEnumerable<DrawnTable> LoadTables(int departmentId);
        void UpdateProductQuantity(OrderProduct orderProduct);
    }
}