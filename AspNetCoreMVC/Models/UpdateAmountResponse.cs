using AspNetCoreMVC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Models
{
    public class UpdateAmountResponse
    {
        public UpdateAmountResponse(OrderItem orderItem, CartViewModel cartViewModel)
        {
            this.orderItem = orderItem;
            this.cartViewModel = cartViewModel;
        }

        public OrderItem orderItem{ get;}
        public CartViewModel cartViewModel{ get; }
    }
}
