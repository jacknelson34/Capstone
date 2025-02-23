using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrazeViewV1
{
    internal class DBSettings
    {
        public string Server { get; init; }
        public string Database { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }

        public DBSettings(string server, string database, string username, string password)
        {
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

    }
}
