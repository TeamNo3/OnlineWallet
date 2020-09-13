using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineWalletServer.Requests.Accounts
{
    public class AddRequest
    {
        public float MoneyAmount { get; set; }

        public uint Account { get; set; }
    }
}
