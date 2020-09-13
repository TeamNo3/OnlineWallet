using System.Threading.Tasks;
using Database;

namespace Services.Transactions
{
    public interface ITransactionPerformer
    {
        Task PerformAsync(Transaction transaction);
    }
}
