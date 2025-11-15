using Data.DbAccessLayer;
using Data.Entities;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shared
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : AEntityBase
    {
        private bool disposedValue;
        private SchoolContext _context;
        
        public RepositoryBase(SchoolContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var entities = await _context.Set<T>().AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<List<T>> GetAsync(Func<T, bool> predicate)
        {
            var entities = await _context.Set<T>().AsNoTracking().ToListAsync();
            
            return entities.ToList();
        }


        public async Task InsertAsync(T entity, Func<T, bool> predicate)
        {
            var entities = await _context.Set<T>().AsNoTracking().ToListAsync();

            if (!entities.Any()) 
            { 
                await _context.AddAsync(entity);
                _context.Entry(entity).State = EntityState.Added;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entityToDelete = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if(entityToDelete != null)
            {
                _context.Remove(entityToDelete);
                _context.Entry(entityToDelete).State = EntityState.Deleted;
            }
        }

        public async Task Update(T entity)
        {
            var entityToUpdate = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (entityToUpdate == null)
            {
                return;
            }

            _context.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;

            entityToUpdate = entity;
            _context.Update(entityToUpdate);

        }

        public async Task SaveChanges()
        {
            var modifiedEntries = _context.ChangeTracker
                .Entries()
                .Where(x => x.State != EntityState.Unchanged);

            var hasModifications = modifiedEntries.Any();

            modifiedEntries = _context.ChangeTracker
              .Entries()
              .Where(x => x.State == EntityState.Modified ||
              x.State == EntityState.Added);

            foreach (var entry in modifiedEntries)
            {
                if (entry != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((AEntityBase)entry.Entity).CreatedBy = "";
                        ((AEntityBase)entry.Entity).CreatedAt = DateTime.UtcNow;
                        ((AEntityBase)entry.Entity).UpdatedBy = "";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.UtcNow;

                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        ((AEntityBase)entry.Entity).UpdatedBy = "";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            if (hasModifications)
            {
                await _context.SaveChangesAsync();
            }
        }

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
