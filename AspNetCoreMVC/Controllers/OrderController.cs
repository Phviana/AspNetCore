using AspNetCoreMVC.Models;
using AspNetCoreMVC.Models.ViewModel;
using AspNetCoreMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;

        public OrderController(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<IActionResult> Carousel()
        {
            var listProducts = await productRepository.GetProductsAsync();

            return View(listProducts);
        }

        public async Task<IActionResult> SearchProduct(string search)
        {
            return View(await productRepository.GetProductsAsync(search));
        }

        public async Task<IActionResult> Cart(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                await orderRepository.AddItemAsync(code);
            }

            var order = await orderRepository.GetOrderAsync();
            List<OrderItem> itens = order.Itens;
            CartViewModel cartViewModel = new CartViewModel(itens);
            return View(cartViewModel);
        }
        public async Task<IActionResult> Register()
        {
            var order = await orderRepository.GetOrderAsync();

            if (order == null)
            {
                return RedirectToAction("Carousel");
            }

            return View(order.Register);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resume(Register register)
        {
            if (ModelState.IsValid)
            {
                Order order = await orderRepository.UpdateRegisterAsync(register);
                return View(order);
            }

            return RedirectToAction("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<UpdateAmountResponse> UpdateAmount([FromBody]OrderItem orderItem)
        {
            return await orderRepository.UpdateAmountAsync(orderItem);
        }
    }
}
