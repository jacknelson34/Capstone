using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;

namespace SqlServerConnector
{
    public class DatabaseQueries : IAsyncDisposable, IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection? _connection;
        private bool _disposed;

        public DatabaseQueries(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _connection = new SqlConnection(_connectionString);
        }

        private async Task EnsureConnectionOpenAsync()
        {
            if (_connection?.State != ConnectionState.Open)
            {
                Console.WriteLine("Opening new database connection...");
                _connection = new SqlConnection(_connectionString);
                await _connection.OpenAsync();
                Console.WriteLine($"Connection opened. State: {_connection.State}");
            }
        }

        private async Task CleanupAsync()
        {
            if (_connection != null)
            {
                Console.WriteLine("\nStarting connection cleanup...");
                if (_connection.State == ConnectionState.Open)
                {
                    Console.WriteLine("Closing open connection...");
                    await _connection.CloseAsync();
                    Console.WriteLine("Connection closed successfully.");
                }
                Console.WriteLine("Disposing connection...");
                await _connection.DisposeAsync();
                _connection = null;
                Console.WriteLine("Connection disposed.");
            }
        }

        public async Task<List<string>> FetchIdentifiersAsync()
        {
            var commonIdentifiers = new HashSet<string>();

            try
            {
                await EnsureConnectionOpenAsync();

                // Query CSVDB identifiers
                using (var command = new SqlCommand(@"
                    SELECT DISTINCT
                        TRIM(
                            CASE
                                WHEN CHARINDEX('HD', SourceFile) > 0 OR CHARINDEX('Export', SourceFile) > 0
                                THEN LEFT(SourceFile, CHARINDEX('.', SourceFile) - 1)
                                ELSE SourceFile
                            END
                        )
                    FROM CSVDB", _connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            commonIdentifiers.Add(reader.GetString(0));
                        }
                    }
                }

                // Query Images identifiers
                using (var command = new SqlCommand(@"
                    SELECT DISTINCT
                        TRIM(
                            CASE
                                WHEN CHARINDEX('HD', ImageName) > 0 OR CHARINDEX('Export', ImageName) > 0
                                THEN LEFT(ImageName, CHARINDEX('.', ImageName) - 1)
                                ELSE ImageName
                            END
                        )
                    FROM Images", _connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var imageIdentifiers = new HashSet<string>();
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            imageIdentifiers.Add(reader.GetString(0));
                        }
                    }

                    // Keep only common identifiers
                    commonIdentifiers.IntersectWith(imageIdentifiers);
                }
            }
            catch (Exception)
            {
                await CleanupAsync();
                throw;
            }

            var result = new List<string>(commonIdentifiers);
            result.Sort();
            return result;
        }

        public async Task DisplayDataForIdentifierAsync(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentException("Identifier cannot be null or empty.", nameof(identifier));
            }

            try
            {
                await EnsureConnectionOpenAsync();

                // Query image data
                using (var command = new SqlCommand(
                    "SELECT ImageData, ImageName FROM Images WHERE ImageName LIKE @imageName",
                    _connection))
                {
                    command.Parameters.AddWithValue("@imageName", identifier + ".png");
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync() && !reader.IsDBNull(1))
                        {
                            var imageName = reader.GetString(1);
                            Console.WriteLine($"\nImage found: {imageName}");
                            Console.WriteLine("Image data is available (binary content not displayed)");
                        }
                        else
                        {
                            Console.WriteLine($"\nNo image found for {identifier}.png");
                        }
                    }
                }

                // Query CSV data
                using (var command = new SqlCommand(
                    "SELECT * FROM CSVDB WHERE SourceFile LIKE @sourceFile",
                    _connection))
                {
                    command.Parameters.AddWithValue("@sourceFile", identifier + ".txt");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine($"\nNo metadata found for {identifier}.txt");
                            return;
                        }

                        Console.WriteLine("\nMetadata from CSVDB:");
                        Console.WriteLine("===================");

                        var dataTable = new DataTable();
                        dataTable.Load(reader);

                        // Display metadata in a formatted table
                        DisplayFormattedTable(dataTable);
                    }
                }
            }
            catch (Exception)
            {
                await CleanupAsync();
                throw;
            }
        }

        private void DisplayFormattedTable(DataTable dataTable)
        {
            if (dataTable.Columns.Count == 0)
            {
                Console.WriteLine("No columns found in the metadata.");
                return;
            }

            // Calculate column widths
            var columnWidths = new int[dataTable.Columns.Count];
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                columnWidths[i] = dataTable.Columns[i].ColumnName?.Length ?? 0;

                foreach (DataRow row in dataTable.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        var cellValue = row[i]?.ToString() ?? "NULL";
                        columnWidths[i] = Math.Max(columnWidths[i], cellValue.Length);
                    }
                }
            }

            // Print header
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                Console.Write($"{dataTable.Columns[i].ColumnName?.PadRight(columnWidths[i] + 2)}");
            }
            Console.WriteLine();

            // Print separator
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                Console.Write(new string('-', columnWidths[i]) + "  ");
            }
            Console.WriteLine();

            // Print data rows
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var value = row.IsNull(i) ? "NULL" : row[i]?.ToString() ?? "NULL";
                    Console.Write($"{value.PadRight(columnWidths[i] + 2)}");
                }
                Console.WriteLine();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                Console.WriteLine("\nDisposing DatabaseQueries instance...");
                await CleanupAsync();
                _disposed = true;
                Console.WriteLine("DatabaseQueries instance disposed.");
            }
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Console.WriteLine("\nDisposing DatabaseQueries instance (sync)...");
                CleanupAsync().GetAwaiter().GetResult();
                _disposed = true;
                Console.WriteLine("DatabaseQueries instance disposed (sync).");
            }
            GC.SuppressFinalize(this);
        }

        ~DatabaseQueries()
        {
            Dispose();
        }
    }
}