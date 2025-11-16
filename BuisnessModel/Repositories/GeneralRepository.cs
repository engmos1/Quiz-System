using BuisnessModel.Interfaces;
using DataAccess.Context;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace ExaminationSystem.Repositories
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : BaseModel

    {
        QuizSystemContext _context;
        DbSet<T> _dbSet; 
        public GeneralRepository(QuizSystemContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public  IQueryable<T> GetAll()
        {
            var res =  _dbSet.Where(x => !x.IsDeleted);
            return res;
        }
        public async Task<T> GetByID(int id)
        {
            var res = await _dbSet.Where(c => c.ID == id).FirstOrDefaultAsync();
            return res;
        }
        public async Task<T> GetByIDWithTracking(int id)
        {
            var res = await _dbSet.AsTracking().Where(c => c.ID == id).FirstOrDefaultAsync();
            return res;
        }
        public async Task Add(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var res = await GetByIDWithTracking(id);
            res.IsDeleted = true;   

            await _context.SaveChangesAsync();
        }
        public void UpdateInclude(T entity , params string[] modifiedParams)
        {
            if(!_dbSet.Any(x => x.ID == entity.ID))
                { return; }

            var local = _dbSet.Local.FirstOrDefault(x => x.ID == entity.ID);
            EntityEntry entityEntry;

            if(local == null)
            {
                entityEntry = _context.Entry(entity);
            }
            else
            {
                entityEntry = _context.ChangeTracker.Entries<T>().FirstOrDefault(x => x.Entity.ID == entity.ID);
            }

            foreach( var prop in entityEntry.Properties)
            {
                if(modifiedParams.Contains(prop.Metadata.Name))
                {
                    prop.CurrentValue = entity.GetType().GetProperty(prop.Metadata.Name).GetValue(entity);
                    prop.IsModified = true; 
                }
            }
            _context.SaveChanges();
        }

    }
}

