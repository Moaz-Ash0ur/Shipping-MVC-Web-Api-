using Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(); 
        IQueryable<T> GetAllQueryable();
        T GetByID(Guid Id);
        bool Insert(T entity);
        bool Insert(T entity,out Guid Id);
        bool Delete(Guid ID);
        bool ChangeStatus(Guid ID , Guid userID , int status = 1);
        bool Update(T entity);
        bool Update(Guid Id, Action<T> UpdateFiled);
        void Save();
        T GetFirst(Expression<Func<T, bool>> filterExpression);
        List<T> GetList(Expression<Func<T, bool>> filterExpression);

    }




}
