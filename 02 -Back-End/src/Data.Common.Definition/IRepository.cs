using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Common.Definition
{
    public interface IRepository<T>
        where T : class
    {
        IUnitOfWork UnitOfWork { get; set; }

        object Mapper(object entity, object entity2);
        void Add(T item);

        T AddItem(T item);

        IEnumerable<T> GetForEdit(Expression<Func<T, bool>> filter, IEnumerable<T> entity);
        bool All(Expression<Func<T, bool>> predicate);

        bool Any(Expression<Func<T, bool>> predicate);

        long Count(Expression<Func<T, bool>> filter = null);

        T Get(Expression<Func<T, bool>> specification);

        IQueryable<T> GetAll();
        IQueryable<T> GetFiltered(Expression<Func<T, bool>> filter, bool asNoTracking = false);
        void Remove(T item);

        void Update(T entity);
        void Edit(T entity);

        IQueryable<T> GetQueryable();
    }
}