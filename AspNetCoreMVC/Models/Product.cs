using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Models
{
    public class Product : BaseModel
    {
        public Product()
        {

        }

        [Required]
        public string Code { get; private set; }
        [Required]
        public string Name { get; private set; }
        [Required]
        public decimal Price { get; private set; } 
        [Required]
        public Category Category { get; private set; }

        public Product(string code, string name, decimal price, Category category)
        {
            this.Code = code;
            this.Name = name;
            this.Price = price;
            this.Category = category;
        }
    }
}
