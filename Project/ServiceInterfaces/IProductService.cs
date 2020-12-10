using Project.DAL.DataAccess;
using Project.DAL.JsonAccess;
using Project.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces
{
    public interface IProductService<T> where T : IRepository
    {
        Task<IEnumerable<Product>> GetAllProductAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Product product);
        Task CreateProductAsync(Product product);
    }
}
