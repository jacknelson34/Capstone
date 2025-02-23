using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;

namespace SqlServerConnector
{
    public class DatabaseConnection
    {
        private readonly ConnectionSettings _settings;
        private readonly string _connectionString;
        private const int MaxRetries = 3;
        private const int RetryDelayMs = 2000; // 2 seconds delay between retries

        public string ConnectionString => _connectionString;

        public DatabaseConnection(ConnectionSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _connectionString = BuildConnectionString();
        }

        private string BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = _settings.Server,
                InitialCatalog = _settings.Database,
                UserID = _settings.Username,
                Password = _settings.Password,
                TrustServerCertificate = true,
                ConnectTimeout = 30
            };

            return builder.ConnectionString;
        }

        public async Task<bool> TestConnectionAsync()
        {
            Console.WriteLine("Attempting to connect to SQL Server...");
            Console.WriteLine($"Server: {_settings.Server}");
            Console.WriteLine($"Database: {_settings.Database}");

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        if (attempt > 1)
                        {
                            Console.WriteLine($"\nRetry attempt {attempt} of {MaxRetries}...");
                            await Task.Delay(RetryDelayMs);
                        }

                        await connection.OpenAsync();
                        Console.WriteLine($"Server Version: {connection.ServerVersion}");
                        Console.WriteLine($"Database: {connection.Database}");
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"SQL Server Error: {ex.Message}");
                        Console.WriteLine($"Error Number: {ex.Number}");
                        Console.WriteLine($"Error State: {ex.State}");
                        Console.ResetColor();

                        if (attempt == MaxRetries)
                        {
                            Console.WriteLine("Maximum retry attempts reached.");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"General Error: {ex.Message}");
                        Console.WriteLine($"Error Type: {ex.GetType().Name}");
                        Console.ResetColor();

                        if (attempt == MaxRetries)
                        {
                            Console.WriteLine("Maximum retry attempts reached.");
                            return false;
                        }
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            await connection.CloseAsync();
                        }
                    }
                }
            }

            return false;
        }
    }
}