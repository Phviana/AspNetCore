using AspNetCoreMVC.Areas.Identity.Data;
using AspNetCoreMVC.Models;
using AspNetCoreMVC.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Repository
{

    public interface IOrderRepository
    {
        Task<Order> GetOrderAsync();
        Task AddItemAsync(string code);
        Task<UpdateAmountResponse> UpdateAmountAsync(OrderItem orderItem);
        Task<Order> UpdateRegisterAsync(Register register);
    }
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHttpHelper httpHelper;
        private readonly IRegisterRepository registerRepository;
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly IReportHelper reportHelper;

        public OrderRepository(ApplicationContext context, 
                IHttpContextAccessor httpContextAccessor,
                IConfiguration configuration,
                IHttpHelper sessionHelper,
                IRegisterRepository registerRepository,
                UserManager<AppIdentityUser> userManager,
                IReportHelper reportHelper) : base(context, configuration)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.httpHelper = sessionHelper;
            this.registerRepository = registerRepository;
            this.userManager = userManager;
            this.reportHelper = reportHelper;
        }

        public async Task AddItemAsync(string code)
        {
            var product = await context.Set<Product>()
                                    .Where(p => p.Code == code)
                                    .SingleOrDefaultAsync();

            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            var order = await GetOrderAsync();

            var orderItem = await context.Set<OrderItem>()
                                    .Where(o => o.Product.Code == code
                                        && o.Order.Id == order.Id)
                                        .SingleOrDefaultAsync();

            if (orderItem == null)
            {
                orderItem = new OrderItem(order, product, 1, product.Price);
                await context.Set<OrderItem>()
                            .AddAsync(orderItem);

                await context.SaveChangesAsync();
            }

                                    
        }

        public async Task<Order> GetOrderAsync()
        {
            var orderId = httpHelper.GetOrderId();
            var order = await dbSet
                            .Include(p => p.Itens)
                                .ThenInclude(i => i.Product)
                            .Include(c => c.Register)
                        .Where(o => o.Id == orderId)
                        .SingleOrDefaultAsync();

            if (order == null)
            {
                var claimsPrincipal = httpContextAccessor.HttpContext.User;
                // var clientId = userManager.GetUserId(claimsPrincipal);

                var clientId = claimsPrincipal.FindFirst("sub")?.Value;
                order = new Order(clientId);
                await dbSet.AddAsync(order);
                await context.SaveChangesAsync();
                httpHelper.SetOrderId(order.Id);
            }

            return order;
        }

        

        public async Task<UpdateAmountResponse> UpdateAmountAsync(OrderItem orderItem)
        {
            var orderItemDb = await GetOrderItemAsync(orderItem.Id);

            if (orderItemDb != null)
            {
                orderItemDb.UpdateAmount(orderItem.Amount);

                if (orderItem.Amount == 0)
                {
                    await RemoveOrderItemAsync(orderItem.Id);
                }

                await context.SaveChangesAsync();

                var order = await GetOrderAsync();

                var cartViewModel = new CartViewModel(order.Itens);

                return new UpdateAmountResponse(orderItemDb, cartViewModel);
            }

            throw new ArgumentException("Order Not Found");
        }

        public async Task<Order> UpdateRegisterAsync(Register register)
        {
            var order = await GetOrderAsync();
            await registerRepository.UpdateAsync(order.Register.Id, register);
            httpHelper.ResetOrderId();
            httpHelper.SetRegister(order.Register);
            await reportHelper.CreateReport(order);
            return order;
        }

        private async Task<OrderItem> GetOrderItemAsync(int orderItemId)
        {
            return
            await context.Set<OrderItem>()
                .Where(ip => ip.Id == orderItemId)
                .SingleOrDefaultAsync();
        }

        private async Task RemoveOrderItemAsync(int orderItemId)
        {
            context.Set<OrderItem>()
                .Remove(await GetOrderItemAsync(orderItemId));
        }

    }   
}
