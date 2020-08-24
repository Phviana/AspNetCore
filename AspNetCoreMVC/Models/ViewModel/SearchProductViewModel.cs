using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Models.ViewModel
{
    public class SearchProductViewModel
    {
        public SearchProductViewModel(IList<Product> product, string search)
        {
            Product = product;
            Search = search;
        }

        public IList<Product> Product { get; }
        public string Search { get; set; }
    }
}
