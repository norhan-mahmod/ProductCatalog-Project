using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProductRepository :IGenericRepository<Product>
    {
        List<Product> GetProductsByCategory(int categoryId);
        List<Category> GetCategories();
    }
}
