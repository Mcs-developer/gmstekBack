using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GMSTEK.Models;

namespace GMSTEK.Data
{
    public interface IRepository<T> where T: class
    {
        void Create(T entity);
        void Delete(T id);
        void Update(T entidad);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
    }
}
