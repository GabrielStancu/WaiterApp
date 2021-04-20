using System;

namespace Infrastructure.Exceptions
{
    public class ConnectionStringException: Exception
    {
        public ConnectionStringException() : base("Bad connection string. Please configure the database settings.")
        {

        }
    }
}
