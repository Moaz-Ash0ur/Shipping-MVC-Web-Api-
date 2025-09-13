using Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {

        //class for get all repository and use same context to ensure success for transation
        //to save all and confirm for data consitnecy and integrity

        //make generic fun to can add any Rpo and use inseid 
        //make func for BeginTrasnstion
        //Fun for Commit
        //Fun for SaveChabge

        IGenericRepository<T> GetRepository<T>() where T : BaseTable;
        Task BeginTransaction();
        Task CommitChanges();
        Task Rollback();
        Task<int> SaveChangesAsync();

    }




}
