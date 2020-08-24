using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Models
{
    public class Category : BaseModel
    {
        public Category() { }

        public Category(string name)
        {
            Name = name;
        }

        [Required]
        public string Name { get; private set; }
    }
}
