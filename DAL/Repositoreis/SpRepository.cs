using DAL.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositoreis
{
    public class SpRepository : ISpRepository
    {
        private readonly ShippingContext _context;

        public SpRepository(ShippingContext context)
        {
            _context = context;
        }

        public async Task<List<T>> ExecuteAsync<T>(string storedProc, params object[] parameters) where T : class
        {
            return await _context.Set<T>()
                .FromSqlRaw(storedProc, parameters)
                .ToListAsync();
        }

        public async Task<T?> ExecuteSingleAsync<T>(string storedProc, params object[] parameters) where T : class
        {
            return await _context.Set<T>()
                .FromSqlRaw(storedProc, parameters)
                .FirstOrDefaultAsync();
        }


    }






}
