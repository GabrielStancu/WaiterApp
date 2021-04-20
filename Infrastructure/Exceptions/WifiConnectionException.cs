using System;

namespace Infrastructure.Exceptions
{
    public class WifiConnectionException : Exception
    {
        public WifiConnectionException(string msg):base(msg)
        {
        }
    }
}
