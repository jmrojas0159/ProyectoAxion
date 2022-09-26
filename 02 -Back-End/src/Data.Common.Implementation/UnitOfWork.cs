using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using Data.Common.Definition;

namespace Data.Common.Implementation
{
    public class UnitOfWork : DbContext, IQueryableUnitOfWork
    {
        public UnitOfWork(string connectionString)
            : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var sb = new StringBuilder();
                const string template = "Saving Entity validation error Class {0} :: Property {1} :: Error {2}";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var error = string.Format(CultureInfo.CurrentCulture,
                            template,
                            validationErrors.Entry.Entity.GetType().FullName,
                            validationError.PropertyName,
                            validationError.ErrorMessage);

                        sb.AppendLine(error);
                    }
                throw new Exception(sb.ToString(), dbEx);
            }
        }

        public int CommitInt()
        {
            try
            {
                return SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var sb = new StringBuilder();
                const string template = "Saving Entity validation error Class {0} :: Property {1} :: Error {2}";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var error = string.Format(CultureInfo.CurrentCulture,
                            template,
                            validationErrors.Entry.Entity.GetType().FullName,
                            validationError.PropertyName,
                            validationError.ErrorMessage);

                        sb.AppendLine(error);
                    }

                throw new Exception(sb.ToString(), dbEx);
            }
        }

        public void CommitAndRefreshChanges()
        {
            var saveFailed = false;
            do
            {
                try
                {
                    SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    saveFailed = true;
                    var sb = new StringBuilder();
                    const string template = "Saving Entity validation error Class {0} :: Property {1} :: Error {2}";
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            var error = string.Format(CultureInfo.CurrentCulture,
                                template,
                                validationErrors.Entry.Entity.GetType().FullName,
                                validationError.PropertyName,
                                validationError.ErrorMessage);

                            sb.AppendLine(error);
                        }
                }
                catch (DbUpdateConcurrencyException dbEx)
                {
                    saveFailed = true;
                    dbEx.Entries.ToList().ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));
                }
            } while (saveFailed);
        }

        public void RollbackChanges()
        {
            ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public void ChangeDatabase(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class
        {
            Entry(original).CurrentValues.SetValues(current);
        }

        public void Attach<TEntity>(TEntity item) where TEntity : class
        {
            var entry = Entry(item);
            entry.State = EntityState.Modified;
        }

        public IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : class
        {
            Entry(item).State = EntityState.Modified;
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }
    }
}