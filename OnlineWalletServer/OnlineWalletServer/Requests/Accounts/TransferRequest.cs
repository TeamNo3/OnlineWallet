using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineWalletServer.Requests.Accounts
{
    public class TransferRequest
    {
        public float MoneyAmount { get; set; }

        public uint From { get; set; }

        public uint To { get; set; }
    }
}
