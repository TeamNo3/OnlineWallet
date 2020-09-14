using System;
using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore;
using Services.Transactions.Exceptions;

namespace Services.Transactions
{
    public class TransactionPerformer : ITransactionPerformer
    {
        private WalletDbContext _dbContext;

        public TransactionPerformer(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task PerformAsync(Transaction transaction)
        {
            var from = await _dbContext.Account.FirstOrDefaultAsync(t => t.Id == transaction.From);
            var to = await _dbContext.Account.FirstOrDefaultAsync(t => t.Id == transaction.To);

            if (from == null || to == null) throw new AccountsNotFoundException();
            if (from.IsFrozen || to.IsFrozen) throw new AccountsFrozenException();

            from.Balance -= transaction.Amount;
            to.Balance += transaction.Amount;
        }
    }
}