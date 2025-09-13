using DAL.Contracts;
using DAL.Exeptions;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DAL.Repositoreis
{
    public class ViewRepository<T> : IViewRepository<T> where T : class
    {

        private readonly ShippingContext _Context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<ViewRepository<T>> _logger;

        public ViewRepository(ShippingContext context, ILogger<ViewRepository<T>> logger)
        {
            _Context = context;
            _dbSet = _Context.Set<T>();
            _logger = logger;
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _dbSet.AsNoTracking().ToList();               
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }

        public T GetByID(Guid Id)
        {
            try
            {
                var entity = _dbSet.Find(Id);
                return entity;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }

        public T GetFirst(Expression<Func<T, bool>> filterExpression)
        {
            try
            {
                var entity = _dbSet.Where(filterExpression).AsNoTracking().FirstOrDefault();
                return entity;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public List<T> GetList(Expression<Func<T, bool>> filterExpression)
        {
            try
            {
                var entity = _dbSet.Where(filterExpression).AsNoTracking().ToList();
                return entity;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }




    }






}
