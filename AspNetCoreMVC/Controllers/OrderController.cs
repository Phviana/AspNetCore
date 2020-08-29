using AspNetCoreMVC.Areas.Identity.Data;
using AspNetCoreMVC.Models;
using AspNetCoreMVC.Models.ViewModel;
using AspNetCoreMVC.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppIdentityUser> userManager;

        public OrderController(IProductRepository productRepository, IOrderRepository orderRepository, UserManager<AppIdentityUser> userManager)
        {
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Register()
        {
            var order = await orderRepository.GetOrderAsync();

            if (order == null)
            {
                return RedirectToAction("Carousel");
            }

            var user = await userManager.GetUserAsync(this.User);

            order.Register.Email = user.Email;
            order.Register.Telephone = user.Telephone;
            order.Register.Name = user.Name;
            order.Register.Address = user.Address;
            order.Register.Complement = user.Complement;
            order.Register.ZipCode = user.ZipCode;
            order.Register.UF = user.State;
            order.Register.Neighborhood = user.Neighborhood;
            order.Register.County = user.County;
            

            return View(order.Register);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resume(Register register)
        {
            if (ModelState.IsValid)
            {

                var user = await userManager.GetUserAsync(this.User);

                user.Email = register.Email;
                user.Telephone = register.Telephone;
                user.Name = register.Name;
                user.Address = register.Address;
                user.Complement = register.Complement;
                user.ZipCode = register.ZipCode;
                user.State = register.UF;
                user.Neighborhood = register.Neighborhood;
                user.County = register.County;

                await userManager.UpdateAsync(user);

                Order order = await orderRepository.UpdateRegisterAsync(register);
                return View(order);
            }

            return RedirectToAction("Register");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<UpdateAmountResponse> UpdateAmount([FromBody]OrderItem orderItem)
        {
            return await orderRepository.UpdateAmountAsync(orderItem);
        }
    }
}
