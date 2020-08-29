using AspNetCoreMVC.Areas.Identity.Data;
using AspNetCoreMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Repository
{
    public class HttpHelper : IHttpHelper
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly UserManager<AppIdentityUser> userManager;

        public IConfiguration Configuration { get; }

        public HttpHelper(IHttpContextAccessor contextAccessor, IConfiguration configuration, UserManager<AppIdentityUser> userManager)
        {
            this.contextAccessor = contextAccessor;
            Configuration = configuration;
            this.userManager = userManager;
        }

        public int? GetOrderId()
        {
            return contextAccessor.HttpContext.Session.GetInt32($"orderId_{GetClientId()}");
        }

        private string GetClientId()
        {
            var claimsPrincipal = contextAccessor.HttpContext.User;
            var clientId = userManager.GetUserId(claimsPrincipal);
            return clientId;
        }

        public void SetOrderId(int orderId)
        {
            contextAccessor.HttpContext.Session.SetInt32($"orderId_{GetClientId()}", orderId);
        }

        public void ResetOrderId()
        {
            contextAccessor.HttpContext.Session.Remove($"orderId_{GetClientId()}");
        }
        public void SetRegister(Register register)
        {
            string json = JsonConvert.SerializeObject(register.GetClone());
            contextAccessor.HttpContext.Session.SetString("register", json);
        }

        public Register GetRegister()
        {
            string json = contextAccessor.HttpContext.Session.GetString("register");
            if (string.IsNullOrWhiteSpace(json))
                return new Register();

            return JsonConvert.DeserializeObject<Register>(json);
        }
    }
}
