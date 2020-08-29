using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreMVC.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the AppIdentityUser class
    public class AppIdentityUser : IdentityUser
    {
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
