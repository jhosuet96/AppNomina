using Microsoft.EntityFrameworkCore;
using AppNominas.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppNominas.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        public AppNominaContext _context { get; set; }
        public Repository(AppNominaContext context) 
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            //var data = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate).AsNoTracking();
            return query;
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
            return query;
        }

        public T GetByID(int id)
        {
           var value = _context.Set<T>().Find(id);
           return value;
        }        
    }
}
