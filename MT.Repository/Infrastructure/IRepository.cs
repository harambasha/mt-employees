using System;
using System.Collections.Generic;

namespace MT.Repository.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T Update(T entity, Func<T, bool> where);
        void Delete(T entity);
        void Delete(Func<T, bool> where);
        T GetById(Func<T, bool> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByIsActive(Func<T, bool> predicate);
    }
}
