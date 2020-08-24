using AspNetCoreMVC.Models;
using AspNetCoreMVC.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static AspNetCoreMVC.Repository.ProductRepository;

namespace AspNetCoreMVC
{
    public class DataService : IDataService
    {

        public async Task DBInicializeAsync(IServiceProvider provider)
        {
            var context = provider.GetService<ApplicationContext>();

            await context.Database.MigrateAsync();

            if (await context.Set<Product>().AnyAsync())
            {
                return;
            }

            List<Book> books = await GetBooksAsync();

            var produtoRepository = provider.GetService<IProductRepository>();
            await produtoRepository.SaveProductsAsync(books);

        }



        private async Task<List<Book>> GetBooksAsync()
        {
            var json = await File.ReadAllTextAsync("books.json");
            var books = JsonConvert.DeserializeObject<List<Book>>(json);
            return books;
        }

        
    }
}

