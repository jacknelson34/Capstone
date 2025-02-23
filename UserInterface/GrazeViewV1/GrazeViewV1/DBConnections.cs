using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrazeViewV1
{
    internal class DBConnections
    {

        private readonly DBSettings _settings;
        private readonly string _connectionString;
        private const int MaxRetries = 3;
        private const int RetryDelayMs = 2000; // 2 seconds delay between retries

        public string ConnectionString => _connectionString;

        public DBConnections(DBSettings settings)
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
            MessageBox.Show("Attempting to connect to SQL Server...");
            MessageBox.Show($"Server: {_settings.Server}");
            MessageBox.Show($"Database: {_settings.Database}");

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        if (attempt > 1)
                        {
                            MessageBox.Show($"\nRetry attempt {attempt} of {MaxRetries}...");
                            await Task.Delay(RetryDelayMs);
                        }

                        await connection.OpenAsync();
                        MessageBox.Show($"Server Version: {connection.ServerVersion}");
                        MessageBox.Show($"Database: {connection.Database}");
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        MessageBox.Show($"SQL Server Error: {ex.Message}");
                        MessageBox.Show($"Error Number: {ex.Number}");
                        MessageBox.Show($"Error State: {ex.State}");
                        Console.ResetColor();

                        if (attempt == MaxRetries)
                        {
                            MessageBox.Show("Maximum retry attempts reached.");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        MessageBox.Show($"General Error: {ex.Message}");
                        MessageBox.Show($"Error Type: {ex.GetType().Name}");
                        Console.ResetColor();

                        if (attempt == MaxRetries)
                        {
                            MessageBox.Show("Maximum retry attempts reached.");
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
