using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using GMSTEK.Models;

namespace GMSTEK.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDBContext context;

        public Repository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public void Create(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public IEnumerable<T> FindAll()
        {
            return context.Set<T>();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>().Where(expression);
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }

     }
}
