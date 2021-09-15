using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business
{
    public enum TableStatus
    {
        Free, 
        TakenByCurrentWaiter, 
        TakenByOtherWaiter
    }
}
