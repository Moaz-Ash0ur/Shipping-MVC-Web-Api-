using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface ISpRepository
    {
        Task<List<T>> ExecuteAsync<T>(string storedProc, params object[] parameters) where T : class;
    }



}