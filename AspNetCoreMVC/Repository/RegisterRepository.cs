using AspNetCoreMVC.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Repository
{

    public interface IRegisterRepository
    {
        Task<Register> UpdateAsync(int orderId, Register newRegister);
    }
    public class RegisterRepository : BaseRepository<Register>, IRegisterRepository
    {
        

        public RegisterRepository(ApplicationContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task<Register> UpdateAsync(int registerId, Register newRegister)
        {
            var registerDB = dbSet.Where(r => r.Id == registerId)
                                .SingleOrDefault();

            if (registerDB == null)
            {
                throw new ArgumentException("registerDb is null");
            }

            registerDB.Update(newRegister);
            await context.SaveChangesAsync();

            return registerDB;
        }
    }
}
