using System;
using System.ComponentModel.DataAnnotations.Schema;

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

        public override bool Equals(BaseModel other)
        {
            throw new NotImplementedException();
        }
    }
}
