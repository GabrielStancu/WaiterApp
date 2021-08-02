using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Core.Models
{
    public class OrderProduct: BaseModel
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        private double _quantity;
        public double Quantity 
        { 
            get => _quantity; 
            set
            {
                _quantity = value;
                SetProperty<double>(ref _quantity, value);
            }
        }
        //public bool Served { get; set; }
        //public bool Prepared { get; set; }
        public DateTime PlacementTime { get; set; }
        public DateTime? ServingTime { get; set; }
        private Color _color;
        [NotMapped]
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                SetProperty<Color>(ref _color, value);
            }
        }

        public override bool Equals(BaseModel other)
        {
            return other.Id == Id;
        }
    }
}
