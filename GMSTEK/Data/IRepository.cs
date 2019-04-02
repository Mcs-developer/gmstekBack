using System;
using System.Collections.Generic;
using GMSTEK.Models;

namespace GMSTEK.Data
{
    public interface IRepository<T> where T: Entity
    {
        void Create(T entity);
        void Delete(T id);
        void Update(T entidad);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
