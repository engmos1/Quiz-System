using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.Interfaces
{
    public interface IGeneralRepository<T>
    {
        public IQueryable<T> GetAll();
        public Task<T> GetByID(int id);
        public Task<T> GetByIDWithTracking(int id);
        public Task Add(T entity);
        public Task<bool> IsExist(int id);


        public Task Update(T entity);
        public Task Delete(int id);
        public void UpdateInclude(T entity , params string[] modifiedParams);

    }
}
