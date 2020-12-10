
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Project.Entities;
using Microsoft.Extensions.Options;
using Project.DAL.DataAccess;
using Repository;

namespace Project.DAL.JsonAccess
{
    public class ProductJsonAccess : IProductJsonAccess
    {
        private readonly string jsonFilePath;
        public ProductJsonAccess(IOptions<JsonSettings> jsonSettings)
        {
            jsonFilePath = jsonSettings.Value.JsonFilePath;
        }
        public async Task InsertAsync(Product product)
        {
            IEnumerable<Product> products = await this.GetAllAsync();
            product.Id = products.Max(_product => _product.Id) + 1;
            products = products.Append(product);

            await this.WriteToJsonFile(products);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            IEnumerable<Product> products = await this.GetAllAsync();

            Product result = new Product();

            foreach(Product product in products)
            {
                if(product.Id == id)
                {
                    result = product;
                }
            }
            return result;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            List<Product> products = new List<Product>();
            using (StreamReader streamReader = new StreamReader(this.jsonFilePath))
            {
                string json = await streamReader.ReadToEndAsync();
                products = JsonConvert.DeserializeObject<List<Product>>(json);
            }

            return products;
        }

        public async Task UpdateAsync( Product product)
        {
            IEnumerable<Product> products = await this.GetAllAsync();
            var productId = product.Id;
            foreach(Product _product in products)
            {
                if(_product.Id == productId)
                {
                    this.MapProduct(_product, product);
                }
            }

            await this.WriteToJsonFile(products);
        }

        #region MappingHelper
        private void MapProduct(Product oldProduct, Product newProdutct)
        {
            oldProduct.Name = newProdutct.Name;
            oldProduct.Description = newProdutct.Description;
            oldProduct.Category = newProdutct.Category;
            oldProduct.Producer = newProdutct.Producer;
            oldProduct.Supplier = newProdutct.Supplier;
            oldProduct.Price = newProdutct.Price;
        }
        #endregion

        #region WriteToJsonFile
        private async Task WriteToJsonFile(IEnumerable<Product> products)
        {
            string jsonProducts = JsonConvert.SerializeObject(products);

            using (StreamWriter streamWriter = new StreamWriter(this.jsonFilePath))
            {
                await streamWriter.WriteAsync(jsonProducts);
            }
        }
        #endregion
    }
}
