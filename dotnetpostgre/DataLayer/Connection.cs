using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetpostgre
{
    public class Connection
    {
        IConfiguration _iconfiguration;

        public Connection(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
    }
}
