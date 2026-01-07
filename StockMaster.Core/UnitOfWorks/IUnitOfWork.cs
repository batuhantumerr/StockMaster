using System;
using System.Collections.Generic;
using System.Text;

namespace StockMaster.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task CommitAsync(); // SaveChangesAsync
        void Commit();      // SaveChanges
    }
}
