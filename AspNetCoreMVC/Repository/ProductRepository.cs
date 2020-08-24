using AspNetCoreMVC.Models;
using AspNetCoreMVC.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Repository
{

    public interface IProductRepository
    {
        Task SaveProductsAsync(List<ProductRepository.Book> books);
        Task<IList<Product>> GetProductsAsync();
        Task<SearchProductViewModel> GetProductsAsync(string search);
    }
    public class ProductRepository : BaseRepository<Product> , IProductRepository
    {
        public ProductRepository(ApplicationContext context, 
            IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task<IList<Product>> GetProductsAsync()
        {
            return await dbSet
               .Include(prod => prod.Category)
               .ToListAsync();

        }

        public async Task SaveProductsAsync(List<Book> books)
        {
            await SaveCategories(books);

            foreach (var book in books)
            {
                var category =
                    await context.Set<Category>()
                        .SingleAsync(c => c.Name == book.Category);

                if (!await dbSet.Where(p => p.Code == book.Code).AnyAsync())
                {
                    await dbSet.AddAsync(new Product(book.Code, book.Name, book.Price, category));
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task SaveCategories(List<Book> book)
        {
            var categories =
                book
                    .OrderBy(l => l.Category)
                    .Select(l => l.Category)
                    .Distinct();

            foreach (var categoryName in categories)
            {
                var categoryDB =
                    await context.Set<Category>()
                    .SingleOrDefaultAsync(c => c.Name == categoryName);
                if (categoryDB == null)
                {
                    await context.Set<Category>()
                        .AddAsync(new Category(categoryName));
                }
            }
            await context.SaveChangesAsync();
        }

        public async Task<SearchProductViewModel> GetProductsAsync(string search)
        {
            IQueryable<Product> query = dbSet;

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Name.Contains(search));
            }

            query = query
                .Include(prod => prod.Category);

            return new SearchProductViewModel(await query.ToListAsync(), search);
        }

        public class Book
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }
            public string Subcategory { get; set; }

        }
    }
}
