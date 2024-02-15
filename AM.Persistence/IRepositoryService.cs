using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using System.Threading;

namespace AM.Persistence
{
    public interface IRepositoryService<T>
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, string includeProp = null, bool noTracking = true, CancellationToken cancelationToken = default);
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null, string includeProp = null, bool noTracking = true, CancellationToken cancelationToken = default);
        IQueryable<T> GetIQueryable(Expression<Func<T, bool>> filter = null, string includeProp = null, bool noTracking = true);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, string includeProp = null, bool noTracking = true, CancellationToken cancelationToken = default);
        Task Add(T entity, CancellationToken cancelationToken = default);
        Task AddRange(IEnumerable<T> entity, CancellationToken cancelationToken = default);
        bool IsAny();
        void Remove(T entity, string includeprop = null, bool tracking = false);
        void RemoveRange(IEnumerable<T> entities, string includProp = null);
        Task<IEnumerable<T>> GetFilterData(Expression<Func<T, bool>> filter = null, string prop = null, string val = null, string includeprop = null, CancellationToken cancelationToken = default);
        int GetCount();
        Task<IEnumerable<T>> CheckDatabaseChanges();
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includePropperties);
        IQueryable<T> GetFirstOrDefaultIQuery(Expression<Func<T, bool>> filter, string inCludePropperties = null, bool noTracking = true, CancellationToken cancelationToken = default);
        Task<IEnumerable<T>> GetPagingData(Expression<Func<T, bool>> predicate, int skip, int take, string includeProp = null, CancellationToken cancelationToken = default);
        IQueryable<T> DynamicFilter(Dictionary<object, object> filters, string includeProp = null);
    }
}
