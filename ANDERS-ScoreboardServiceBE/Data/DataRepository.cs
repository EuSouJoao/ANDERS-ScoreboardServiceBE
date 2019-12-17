using ANDERS_ScoreboardServiceBE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ANDERS_ScoreboardServiceBE.Data
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        private readonly ScoreboardContext _context;

        public DataRepository(ScoreboardContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> SaveAsync(T entity)
        {
            await _context.SaveChangesAsync();
            return entity;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
