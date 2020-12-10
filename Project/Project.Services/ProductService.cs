using Microsoft.Extensions.Options;
using Project.DAL.DataAccess;
using Project.DAL.JsonAccess;
using Project.Entities;
using Repository;
using ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public class ProductService<T> : IProductService<T> where T : IRepository
    {
        private readonly T _repository;

        public ProductService(T repository)
        {
            this._repository = repository;
        }
        
        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            var products = await this._repository.GetAllAsync();
            return products;
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await this._repository.GetByIdAsync(id);
            return product;
        }
        public async Task UpdateProductAsync(Product product)
        {
             await this._repository.UpdateAsync(product);
        }

        public async Task CreateProductAsync(Product product)
        {
             await this._repository.InsertAsync(product);
        }
    }
}
