using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Order : BaseModel
    {
        [ForeignKey("Waiter")]
        public int WaiterId { get; set; }
        public Waiter Waiter { get; set; }
        [ForeignKey("Table")]
        public int TableId { get; set; }
        public Table Table { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        private double _total;
        public double Total 
        {
            get => _total; 
            set
            {
                _total = value;
                SetProperty<double>(ref _total, value);
            }
        }
        public bool Paid { get; set; }
        public override bool Equals(BaseModel other)
        {
            throw new NotImplementedException();
        }
    }
}
