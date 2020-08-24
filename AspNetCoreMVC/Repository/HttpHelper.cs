using AspNetCoreMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Repository
{
    public class HttpHelper : IHttpHelper
    {
        private readonly IHttpContextAccessor contextAccessor;
        public IConfiguration Configuration { get; }

        public HttpHelper(IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            this.contextAccessor = contextAccessor;
            Configuration = configuration;
        }

        public int? GetOrderId()
        {
            return contextAccessor.HttpContext.Session.GetInt32("orderId");
        }

        public void SetOrderId(int orderId)
        {
            contextAccessor.HttpContext.Session.SetInt32("orderId", orderId);
        }

        public void ResetOrderId()
        {
            contextAccessor.HttpContext.Session.Remove("orderId");
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
