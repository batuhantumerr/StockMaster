using StockMaster.Core.UnitOfWorks;
using StockMaster.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockMaster.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
