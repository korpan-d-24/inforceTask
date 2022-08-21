using DataAccess.Interfaces;
using Domains.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{


    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationContext _context { get; set; }
        private DbSet<T> table = null;
        public GenericRepository(ApplicationContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public Task<List<T>> Get(Expression< Func<T, bool>> filter)
        {
            IQueryable<T> query = table;
            query = query.Where(filter);
            return query.ToListAsync();
        }
        public Task<List<T>> GetAll()
        {
            return table.ToListAsync();
        }
        public ValueTask<T> GetById(object id)
        {
            return table.FindAsync(id);
        }
        public async Task Insert(T obj)
        {
            await table.AddAsync(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(T obj)
        {
            table.Remove(obj);
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public Task<List<T>> GetFiltered(FilteringModel filteringModel, Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = query.Skip(filteringModel.Page * filteringModel.Count);
            if (filteringModel.Count != 0)
            {
                query = query.Take(filteringModel.Count);
            }

            return query.ToListAsync();
        }
    }
}
