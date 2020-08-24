using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Models.ViewModel
{
    public class CartViewModel
    {
        public CartViewModel(List<OrderItem> itens)
        {
            Itens = itens;
        }

        public List<OrderItem> Itens { get; }

        public decimal Total => Itens.Sum(i => i.Amount * i.UnitPrice);
    }
}
