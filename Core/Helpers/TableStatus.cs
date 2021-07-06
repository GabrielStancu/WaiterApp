using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helpers
{
    public enum TableStatus
    {
        Free, 
        TakenByCurrentWaiter, 
        TakenByOtherWaiter
    }
}
