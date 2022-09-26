using System.Collections.Generic;
using System.Data.Entity;

namespace Data.Common.Definition
{
    public interface IQueryableUnitOfWork : IUnitOfWork
    {
        void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class;
        void Attach<TEntity>(TEntity item) where TEntity : class;
        IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class;
        void SetModified<TEntity>(TEntity item) where TEntity : class;
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);
    }
}