using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Dynamic.Core;
using System.Threading;
using System.Reflection;
using RFIM.Persistence;

namespace AM.Persistence
{
    public class RepositoryService<T> : IRepositoryService<T> where T : class
    {
        private AutomationDbService _service;
        private DbSet<T> _dbSet;

        public RepositoryService(AutomationDbService context)
        {
            this._service = context;
            this._dbSet = _service.Set<T>();
            //this._service.ChangeTracker.LazyLoadingEnabled = false;
        }

        #region Sync
        public void Remove(T entity, string includeEntities = null, bool tracking = false)
        {
            if (tracking) _dbSet.AsNoTracking();
            _dbSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities, string includeEntities = null) => _dbSet.RemoveRange(entities);
        public int GetCount()
        {
            return _dbSet.Count();
        }
        public bool IsAny() => _dbSet.Any();

        #endregion


        #region Async
        public async Task<IEnumerable<T>> GetFilterData(Expression<Func<T, bool>> filter = null, string property = null, string val = null, string includeEntities = null, CancellationToken cancelationToken = default)
        {
            IQueryable<T> iquery = (from equi in _dbSet select equi).AsNoTracking();

            if (!COM.IsNull(filter))
            {
                iquery = iquery.Where(filter);
            }

            if (!COM.IsNullOrEmpty(includeEntities))
            {
                foreach (string prop in includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    iquery = iquery.Include(prop);
                }
            }

            if (!COM.IsNullOrEmpty(property))
            {
                string propName = property;
                string value = val;

                iquery = iquery.Where($"{propName} == @0", value);
            }

            return await iquery.ToListAsync(cancelationToken);
        }
        public async Task<IEnumerable<T>> GetPagingData(Expression<Func<T, bool>> predicate, int skip, int take, string includeEntities = null, CancellationToken cancelationToken = default)
        {
            IQueryable<T> iQuery = from entity in _dbSet select entity;

            if (predicate != null)
            {
                iQuery = iQuery.AsNoTracking().Where(predicate);
            }


            if (!COM.IsNullOrEmpty(includeEntities))
            {
                foreach (string prop in includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    iQuery = iQuery.Include(prop);
                }
            }

            iQuery = iQuery.Skip(skip).Take(take);
            return await iQuery.ToListAsync(cancelationToken);
        }
        public Task<IEnumerable<T>> CheckDatabaseChanges()
        {
            //Dictionary<string, List<T>> changedEntities = _service.ChangeTracker.Entries<T>()
            //       .Where(entity => entity.State == EntityState.Modified || entity.State == EntityState.Added || entity.State == EntityState.Deleted)
            //       .GroupBy(entity => entity.State)
            //       .ToDictionary(x => x.Key.ToString(), y => y.OfType<T>().ToList());

            var changedEntities = _service.ChangeTracker.Entries<T>()
                   .Where(entry => entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.State == EntityState.Deleted)
                   .ToList();


            return null;
        }
        public async Task Add(T entity, CancellationToken cancelationToken = default)
        {
            await _dbSet.AddAsync(entity, cancelationToken);
            //await this._service.DisposeAsync();
        }
        public async Task AddRange(IEnumerable<T> entity, CancellationToken cancelationToken = default) => await _dbSet.AddRangeAsync(entity, cancelationToken);
        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, string inCludePropperties = null, bool noTracking = true, CancellationToken cancelationToken = default)
        {
            IQueryable<T> query = null;

            if (!noTracking)
                query = _dbSet;
            else
                query = _dbSet.AsNoTracking();

            query = query.Where(filter);

            if (!COM.IsNullOrEmpty(inCludePropperties))
            {
                foreach (string includeEntities in inCludePropperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeEntities);
                }
            }

            return await query.FirstOrDefaultAsync(cancelationToken);
        }
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, string includeEntities = null, bool noTracking = true, CancellationToken cancelationToken = default)
        {
            IQueryable<T> query;

            if (noTracking)
            {
                query = _dbSet.AsNoTracking();
            }
            else
            {
                query = _dbSet;
            }


            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!COM.IsNullOrEmpty(includeEntities))
            {
                foreach (string prop in includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }

            return await query.ToListAsync(cancelationToken);
        }
        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null, string includeEntities = null, bool noTracking = true, CancellationToken cancelationToken = default)
        {
            IQueryable<T> query;

            if (noTracking)
            {
                query = _dbSet.AsNoTracking();
            }
            else
            {
                query = _dbSet;
            }


            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!COM.IsNullOrEmpty(includeEntities))
            {
                foreach (string prop in includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }

            return await query.ToListAsync(cancelationToken);
        }

        public async Task<bool> IsAnyAsync(Expression<Func<T, bool>> filter, string includeProp = null, bool noTracking = true, CancellationToken cancelationToken = default)
        {
            return await _dbSet.AnyAsync(filter);
        }



        #endregion


        #region Returns IQueryable
        public IQueryable<T> GetIQueryable(Expression<Func<T, bool>> filter = null, string includeEntities = null, bool noTracking = true)
        {
            IQueryable<T> query;

            if (noTracking)
            {
                query = _dbSet.AsNoTracking();
            }
            else
            {
                query = _dbSet;
            }


            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!COM.IsNullOrEmpty(includeEntities))
            {
                foreach (string prop in includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }

            return query;
        }
        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeEntitiesperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (Expression<Func<T, object>> propsExp in includeEntitiesperties)
            {
                query = query.Include(propsExp);
            }

            return query.AsNoTracking();
        }

        public IQueryable<T> GetFirstOrDefaultIQuery(Expression<Func<T, bool>> filter, string inCludePropperties = null, bool noTracking = true, CancellationToken cancelationToken = default)
        {
            IQueryable<T> query = null;

            if (!noTracking)
                query = _dbSet;
            else
                query = _dbSet.AsNoTracking();

            query = query.Where(filter);

            if (!COM.IsNullOrEmpty(inCludePropperties))
            {
                foreach (string includeEntities in inCludePropperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeEntities);
                }
            }

            return query;
        }


        public IQueryable<T> DynamicFilter(Dictionary<object, object> filters, string includeEntities = null)
        {
            IQueryable<T> query = _dbSet;
            query = IncludeProps(query, includeEntities);

            foreach (KeyValuePair<object, object> filter in filters)
            {
                Type type = filter.Key.GetType();
                Type primaryType = type;

                string fieldName = (string)filter.Value;

                var property = type.GetProperty(fieldName);
                var propertyInfo = type.GetType().GetProperty(fieldName);
                object currentObject = filter.Key;

                if (fieldName.Contains("."))
                {
                    string[] fieldNames = fieldName.Split('.');

                    primaryType = null;

                    PropertyInfo currentProperty = null;

                    foreach (string field in fieldNames)
                    {
                        Type curentRecordType = currentObject.GetType();
                        currentProperty = curentRecordType.GetProperty(field);

                        if (fieldNames.First() == field)
                        {
                            primaryType = currentProperty.GetType();
                        }

                        if (currentProperty != null)
                        {
                            if (fieldNames.Last() == field)
                            {
                                break;
                            }

                            var value = currentProperty.GetValue(currentObject, null);
                            currentObject = value;
                        }
                    }

                    property = currentProperty;
                }
                else
                {
                    property = type.GetProperty(fieldName);
                }


                if (property == null)
                {
                    continue;
                }

                ParameterExpression parameter = Expression.Parameter(type, "x");
                Expression combinedExpression = null;


                ConstantExpression constant = Expression.Constant(property);
                MemberExpression propertyAccess = Expression.Property(parameter, property);
                BinaryExpression equality = Expression.Equal(propertyAccess, constant);

                combinedExpression = combinedExpression == null ? equality : Expression.And(combinedExpression, equality);


                if (combinedExpression != null)
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                    query = query.Where(lambda);
                }
            }

            return query;
        }
        private IQueryable<T> IncludeProps(IQueryable<T> query, string includeEntities = null)
        {
            if (!COM.IsNullOrEmpty(includeEntities))
            {
                foreach (string prop in includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }

            return query;
        }


        #endregion
    }
}
