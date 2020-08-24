using System;
using System.Threading.Tasks;

namespace AspNetCoreMVC
{
    interface IDataService
    {
        Task DBInicializeAsync(IServiceProvider provider);
    }
}