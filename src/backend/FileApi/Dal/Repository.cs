
using Dal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Dal
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly BaseDbContext _Context;

        private DbSet<T> _Entities;

        public Repository(BaseDbContext _context)
        {
            _Context = _context;

            _Entities = _context.Set<T>();
        }

        public T Get(long _id)
        {
            return _Entities.SingleOrDefault(s => s.Id == _id);
        }

        public IEnumerable<T> GetAll()
        {
            return _Entities.AsEnumerable();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> _predicate)
        {
            return _Entities.Where(_predicate);
        }

        public T GetFirst(Expression<Func<T, bool>> _predicate)
        {
            return _Entities.FirstOrDefault(_predicate);
        }

        public T Get(Expression<Func<T, bool>> _predicate)
        {
            return _Entities.SingleOrDefault(_predicate);
        }        
        
        public bool Any(Expression<Func<T, bool>> _predicate) =>
            _Entities.Any(_predicate);

        public T Add(T _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return _Entities.Add(_entity).Entity;
        }

        public async Task<T> AddAsync(T _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return (await _Entities.AddAsync(_entity)).Entity;
        }

        public void Remove(T _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _Entities.Remove(_entity);
        }

        public void Remove(long _id)
        {
            _Entities.Remove(Get(_id));
        }
    }
}
