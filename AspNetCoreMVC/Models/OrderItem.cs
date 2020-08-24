using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Models
{
    [DataContract]
    public class OrderItem : BaseModel
    {
        [Required]
        [DataMember]
        public Order Order { get; private set; }
        [Required]
        [DataMember]
        public Product Product { get; private set; }
        [Required]
        [DataMember]
        public int Amount { get; private set; }
        [Required]
        [DataMember]
        public decimal UnitPrice { get; private set; }
        [DataMember]
        public decimal Subtotal => Amount * UnitPrice;

        public OrderItem()
        {

        }

        public OrderItem(Order order, Product product, int amount, decimal unitPrice)
        {
            Order = order;
            Product = product;
            Amount = amount;
            UnitPrice = unitPrice;
        }

        internal void UpdateAmount(int amount)
        {
            Amount = amount;
        }
    }
}
