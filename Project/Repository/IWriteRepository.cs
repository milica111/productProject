using Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IWriteRepository
    {
        Task InsertAsync(Product product);
        Task UpdateAsync(Product product);
    }
}
