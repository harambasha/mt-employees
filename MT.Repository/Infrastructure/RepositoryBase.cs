using System;
using System.Collections.Generic;
using System.Linq;

namespace MT.Repository.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        public List<T> Items;

        protected RepositoryBase(List<T> items)
        {
            items = items;
        }
        public T Add(T entity)
        {
            Items.Add(entity);
            return entity;
        }

        public virtual void Delete(T entity)
        {
            Items.Remove(entity);
        }
        public virtual void Delete(Func<T, bool> selector)
        {
            var itemsToRemove = Items.Where(selector).ToList();
            itemsToRemove.ForEach(i => Items.Remove(i));
        }
        public virtual T GetById(Func<T, bool> where)
        {
            return Items.Where(where).FirstOrDefault();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return Items.ToList();
        }

        public virtual T Update(T entity, Func<T, bool> selector)
        {
            Delete(selector);
            Items.Add(entity);
            return entity;
        }
        public virtual IEnumerable<T> GetByIsActive(Func<T, bool> predicate)
        {
            return Items.ToList().Where(predicate).ToList();
        }
    }
}
