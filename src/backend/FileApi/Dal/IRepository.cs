using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public interface IRepository<T> where T : EntityBase
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>> _predicate);

        T Get(long _id);

        T Get(Expression<Func<T, bool>> _predicate);

        bool Any(Expression<Func<T, bool>> _predicate);

        T GetFirst(Expression<Func<T, bool>> _predicate);

        T Add(T _entity);

        Task<T> AddAsync(T _entity);

        void Remove(T _entity);

        void Remove(long _id);
    }
}
