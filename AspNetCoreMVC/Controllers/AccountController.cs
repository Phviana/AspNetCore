using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Login()
        {
            return Redirect(Url.Content("~/"));
        }
        [Authorize]
        public async Task Logout()
        {
            // update the cookies (Local authentication)
           await HttpContext.SignOutAsync("Cookies");
            // do sign out in OpenIdConnect
           await HttpContext.SignOutAsync("OpenIdConnect");
        }
    }
}