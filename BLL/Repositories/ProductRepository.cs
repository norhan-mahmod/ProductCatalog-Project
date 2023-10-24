using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly CatalogDbContext context;

        public ProductRepository(CatalogDbContext context) : base(context)
        {
            this.context = context;
        }

        public List<Category> GetCategories()
            => context.Categories.ToList();

        public List<Product> GetProductsByCategory(int categoryId)
            => context.Products.Where(product => product.CategoryId == categoryId).ToList();
    }
}
