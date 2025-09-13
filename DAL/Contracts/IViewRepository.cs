using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IViewRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetByID(Guid Id);

         T GetFirst(Expression<Func<T, bool>> filterExpression);

         List<T> GetList(Expression<Func<T, bool>> filterExpression);
    }



}
