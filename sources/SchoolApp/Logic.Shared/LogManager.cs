using Data.DbAccessLayer;
using Data.Entities;
using Logic.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Shared
{
    public class LogManager : ILogManager
    {
        private bool disposedValue;
        private SchoolContext _context;

        public LogManager(SchoolContext context)
        {
            _context = context;
        }

        public async Task AddLogMessage(LogEntity logEntity)
        {
            await _context.AddAsync(logEntity);
        }

        public async Task DeleteLogMessage(int id)
        {
            var table = _context.Set<LogEntity>();

            var entity = await table.FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.Remove(entity);
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public async Task DeleteLogMessages(List<int> ids)
        {
            var table = _context.Set<LogEntity>();

            var entities = await table.Where(x => ids.Contains(x.Id))
                .ToListAsync();

            if (entities.Any())
            {
                _context.RemoveRange(entities);

                entities.ForEach(e => _context.Entry(_context).State = EntityState.Deleted);
            }
        }

        public async Task<List<LogEntity>> GetLogMessages()
        {
            var table = _context.Set<LogEntity>();

            return await table.ToListAsync();
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
