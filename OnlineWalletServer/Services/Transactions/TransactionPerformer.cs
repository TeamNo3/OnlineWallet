using System;
using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore;

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

            if (from == null || to == null) throw new Exception("Accounts not found");

            from.Balance -= transaction.Amount;
            to.Balance += transaction.Amount;
        }
    }
}