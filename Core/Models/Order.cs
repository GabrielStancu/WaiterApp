using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Comanda_Bar")]
    public class Order : BaseModel
    {
        [Column("ID_bon")]
        public new int Id { get; set; }
        [ForeignKey("Waiter")]
        [Column("ID_ospatar")]
        public int WaiterId { get; set; }
        public Waiter Waiter { get; set; }
        [ForeignKey("Table")]
        [Column("ID_masa")]
        public int TableId { get; set; }
        public Table Table { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        private double _total;
        [NotMapped]
        public double Total 
        {
            get => _total; 
            set
            {
                _total = value;
                SetProperty<double>(ref _total, value);
            }
        }
        [NotMapped]
        public bool Paid { get; set; }
        public override bool Equals(BaseModel other)
        {
            throw new NotImplementedException();
        }
    }
}
