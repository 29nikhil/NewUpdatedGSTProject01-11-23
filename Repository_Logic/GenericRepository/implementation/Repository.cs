using Data_Access_Layer.Db_Context;
using Microsoft.EntityFrameworkCore;
using Repository_Logic.GenericRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.GenericRepository.implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Application_Db_Context _context = null;
        private readonly DbSet<T> table = null;

        public Repository()
        {
            this._context = new Application_Db_Context();
            table = _context.Set<T>();
        }

        public void Delete(object id)
        {
            T data = table.Find(id);
            table.Remove(data);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
            _context.SaveChanges();
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                _context.SaveChanges();
            }
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
    }
}
