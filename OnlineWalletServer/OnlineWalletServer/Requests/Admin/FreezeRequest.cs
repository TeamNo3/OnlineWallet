﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineWalletServer.Requests.Admin
{
    public class FreezeRequest
    {
        public byte[] Account { get; set; }
    }
}
