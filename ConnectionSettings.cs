namespace SqlServerConnector
{
    public class ConnectionSettings
    {
        public string Server { get; init; }
        public string Database { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }

        public ConnectionSettings(string server, string database, string username, string password)
        {
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}