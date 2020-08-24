using AspNetCoreMVC.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Repository
{
    public class BaseRepository<T> where T : BaseModel
    {
        protected readonly IConfiguration configuration;
        protected readonly ApplicationContext context;
        protected Microsoft.EntityFrameworkCore.DbSet<T> dbSet;

        public BaseRepository(ApplicationContext context, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.context = context;
            dbSet = context.Set<T>();
        }
    }
}
