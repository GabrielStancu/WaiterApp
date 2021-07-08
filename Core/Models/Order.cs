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
        public double Total { get; set; }
        public bool Paid { get; set; }
        public override bool Equals(BaseModel other)
        {
            throw new NotImplementedException();
        }
    }
}
