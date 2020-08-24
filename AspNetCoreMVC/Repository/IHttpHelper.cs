using AspNetCoreMVC.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Repository
{
    public interface IHttpHelper
    {
        IConfiguration Configuration { get; }
        int? GetOrderId();
        void SetOrderId(int orderId);
        void ResetOrderId();
        void SetRegister(Register register);
        Register GetRegister();
    }
}
