using System;
using dotnetpostgre;
using Microsoft.Extensions.Configuration;

namespace DataLayer
{
    public class User
    {
        public static string GetConnectionString()
        {
            return Startup.ConnectionString;
        }

    }
}
