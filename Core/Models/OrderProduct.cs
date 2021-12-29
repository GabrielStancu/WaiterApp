using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Core.Models
{
    [Table("MARFA_t")]
    public class OrderProduct: BaseModel
    {
        public new int Id { get; set; }
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
        private Color _textColor;
        [NotMapped]
        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                SetProperty<Color>(ref _textColor, value);
            }
        }

        public override bool Equals(BaseModel other)
        {
            return other.Id == Id;
        }
    }
}
