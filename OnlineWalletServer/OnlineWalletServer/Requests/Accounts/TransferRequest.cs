using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineWalletServer.Requests.Accounts
{
    public class TransferRequest
    {
        public float MoneyAmount { get; set; }

        public byte[] From { get; set; }

        public byte[] To { get; set; }
    }
}
