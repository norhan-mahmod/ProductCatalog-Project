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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CatalogDbContext context;

        public GenericRepository(CatalogDbContext context)
        {
            this.context = context;
        }

        public int Add(T entity)
        {
            context.Set<T>().Add(entity);
            return context.SaveChanges();
        }

        public int Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            return context.SaveChanges();
        }

        public List<T> GetAll()
            => context.Set<T>().ToList();

        public T GetById(int id)
            => context.Set<T>().Find(id);

        public int Update(T entity)
        {
            context.Set<T>().Update(entity);
            return context.SaveChanges();
        }
    }
}
