using Microsoft.Extensions.Options;
using Project.Entities;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.DAL.DataAccess
{
    public class ProductDBAccess : IProductDBAccess
    {
        public string ConnectionString { get; }

        public ProductDBAccess(IOptions<DBSettings> dbSettings)
        {
            ConnectionString = dbSettings.Value.ConnectionString;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var result = new List<Product>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[dbo].[GetProducts]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var product = new Product();
                            product.Id = reader.GetInt32("Id");
                            product.Name = reader.GetString("Name");
                            product.Category = reader.GetString("Category");
                            product.Description = reader.GetString("Description");
                            product.Producer = reader.GetString("Producer");
                            product.Supplier = reader.GetString("Supplier");
                            product.Price = reader.GetDouble("Price");
                            result.Add(product);
                        }
                    }
                }
            }

            return result;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = new Product();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("[dbo].[GetProductById]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            product.Id = reader.GetInt32("Id");
                            product.Name = reader.GetString("Name");
                            product.Category = reader.GetString("Category");
                            product.Description = reader.GetString("Description");
                            product.Producer = reader.GetString("Producer");
                            product.Supplier = reader.GetString("Supplier");
                            product.Price = reader.GetDouble("Price");
                        }
                    }
                }

            }

            return product;
        }
        public async Task InsertAsync(Product product)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("[dbo].[InsertProduct]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = product.Name;
                        cmd.Parameters.Add("@category", SqlDbType.NVarChar, 50).Value = product.Category;
                        cmd.Parameters.Add("@description", SqlDbType.NVarChar, 50).Value = product.Description;
                        cmd.Parameters.Add("@producer", SqlDbType.NVarChar, 50).Value = product.Producer;
                        cmd.Parameters.Add("@supplier", SqlDbType.NVarChar, 50).Value = product.Supplier;
                        cmd.Parameters.Add("@price", SqlDbType.Float).Value = product.Price;
                        cmd.Parameters.Add("@id", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        await cmd.ExecuteNonQueryAsync();
                        product.Id = (int)(long)cmd.Parameters["@id"].Value;
                    }
                }

                ts.Complete();
            }

        }
        public async Task UpdateAsync(Product product)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("[dbo].[UpdateProduct]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = product.Name;
                        cmd.Parameters.Add("@category", SqlDbType.NVarChar, 50).Value = product.Category;
                        cmd.Parameters.Add("@description", SqlDbType.NVarChar, 50).Value = product.Description;
                        cmd.Parameters.Add("@producer", SqlDbType.NVarChar, 50).Value = product.Producer;
                        cmd.Parameters.Add("@supplier", SqlDbType.NVarChar, 50).Value = product.Supplier;
                        cmd.Parameters.Add("@price", SqlDbType.Float).Value = product.Price;
                        cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = product.Id;
                        await cmd.ExecuteNonQueryAsync();
                    }

                }

                ts.Complete();
            }

        }
    }
}
