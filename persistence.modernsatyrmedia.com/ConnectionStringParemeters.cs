using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistence.modernsatyrmedia.com
{
    public class ConnectionStringParameters
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IntegratedSecurity { get; set; } = false;  // Optional for Windows Auth
        public int Port { get; set; }  // Optional for databases like MySQL, PostgreSQL
    }

}
