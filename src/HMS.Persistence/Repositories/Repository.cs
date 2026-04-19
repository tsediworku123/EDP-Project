using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HMS.Core.Domain.Interfaces;
using HMS.Core.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HMS.Core.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly HMSDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(HMSDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
