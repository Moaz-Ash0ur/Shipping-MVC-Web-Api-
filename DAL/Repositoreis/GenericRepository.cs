using DAL.Contracts;
using DAL.Exeptions;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repositoreis
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseTable
    {

        private readonly ShippingContext _Context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<IGenericRepository<T>> _logger;

        public GenericRepository(ShippingContext context, ILogger<IGenericRepository<T>> logger)
        {
            _Context = context;
            _dbSet = _Context.Set<T>();
            _logger = logger;
        }


        public IEnumerable<T> GetAll()
        {
            try
            {
                return _dbSet.Where(e => e.CurrentState != 1).ToList();               
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }
        public IQueryable<T> GetAllQueryable()
        {
            try
            {
                return _dbSet.Where(e => e.CurrentState != 1);
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
                var entity = _dbSet.AsNoTracking().SingleOrDefault(e => e.Id == Id);
                return entity;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }

        public bool Insert(T entity)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                _dbSet.Add(entity);
                Save();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
            
        }

        public bool Insert(T entity, out Guid Id)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                _dbSet.Add(entity);
                Save();
                Id = entity.Id;
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public bool Update(T entity)
        {   
            try
            {
                var currentEntity = GetByID(entity.Id);

                entity.UpdatedDate = DateTime.Now;
                entity.CreatedDate = currentEntity.CreatedDate;
                entity.CreatedBy = currentEntity.CreatedBy;

                _Context.Entry(entity).State = EntityState.Modified;
                Save();

                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }

        public bool Update(Guid Id , Action<T> UpdateFiled)
        {
            try
            {
                var entity = _dbSet.SingleOrDefault(s => s.Id == Id);

                UpdateFiled(entity);

                _Context.Entry(entity).State = EntityState.Modified;
                Save();

                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

        }

        public bool Delete(Guid ID)
        {

            try
            {
                var entity = GetByID(ID);

                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }

            return false;
        }

        public bool ChangeStatus(Guid ID, Guid userId, int Status = 1)
        {

            var entity = GetByID(ID);

            if (entity != null)
            {
                entity.CurrentState = Status;
                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedBy = userId;
                _Context.Entry(entity).State = EntityState.Modified;
                Save();
                return true;
            }
            return false;
        }

        public void Save()
        {
            _Context.SaveChanges();
        }

        //Generic Func
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
