using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace GrazeViewV1
{
    // Class to connect the UI to the DB - TODO
    public class DBQueries
    {
    
        private readonly string _connectionString;
        private SqlConnection? _connection;
        private bool _disposed;

        public DBQueries(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _connection = new SqlConnection(_connectionString);
        }

        // New Method for adding uploaded data to DB - Jack
        public async Task InsertUploadAsync(UploadInfo upload, MLData mlData)
        {
            if (upload == null) throw new ArgumentNullException(nameof(upload));
            if (mlData == null) throw new ArgumentNullException(nameof(mlData));

            try
            {
                await EnsureConnectionOpenAsync();

                // Insert UploadInfo into Uploads table
                string insertUploadQuery = @"
            INSERT INTO Uploads (UploadName, SampleDate, SampleTime, UploadTime, SampleLocation, SheepBreed, Comments, ImageData)
            VALUES (@UploadName, @SampleDate, @SampleTime, @UploadTime, @SampleLocation, @SheepBreed, @Comments, @ImageData)";

                using (var command = new SqlCommand(insertUploadQuery, _connection))
                {
                    command.Parameters.AddWithValue("@UploadName", upload.UploadName);
                    command.Parameters.AddWithValue("@SampleDate", upload.SampleDate);
                    command.Parameters.AddWithValue("@SampleTime", upload.SampleTime);
                    command.Parameters.AddWithValue("@UploadTime", upload.UploadTime);
                    command.Parameters.AddWithValue("@SampleLocation", upload.SampleLocation);
                    command.Parameters.AddWithValue("@SheepBreed", upload.SheepBreed);
                    command.Parameters.AddWithValue("@Comments", upload.Comments ?? (object)DBNull.Value);

                    // Convert Image to Byte Array
                    if (upload.ImageFile != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            upload.ImageFile.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            command.Parameters.AddWithValue("@ImageData", ms.ToArray());
                        }
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImageData", DBNull.Value);
                    }

                    await command.ExecuteNonQueryAsync();
                }

                // Insert MLData into MLData table
                string insertMLDataQuery = @"
            INSERT INTO MLData (UploadName, nalePercentage, qufuPercentage, erciPercentage, bubblePercentage)
            VALUES (@UploadName, @nalePercentage, @qufuPercentage, @erciPercentage, @bubblePercentage)";

                using (var command = new SqlCommand(insertMLDataQuery, _connection))
                {
                    command.Parameters.AddWithValue("@UploadName", upload.UploadName);
                    command.Parameters.AddWithValue("@nalePercentage", mlData.nalePercentage ?? ".00");
                    command.Parameters.AddWithValue("@qufuPercentage", mlData.qufuPercentage ?? ".00");
                    command.Parameters.AddWithValue("@erciPercentage", mlData.erciPercentage ?? ".00");
                    command.Parameters.AddWithValue("@bubblePercentage", mlData.bubblePercentage ?? ".00");

                    await command.ExecuteNonQueryAsync();
                }

                MessageBox.Show($"Upload '{upload.UploadName}' and associated ML data inserted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting upload and ML data: {ex.Message}");
                throw;
            }
        }

        // New method for pulling data from DB
        public async Task<List<(UploadInfo, MLData)>> GetUploadsAsync()
        {
            var uploadsWithML = new List<(UploadInfo, MLData)>();

            try
            {
                await EnsureConnectionOpenAsync();

                string query = @"
            SELECT u.UploadName, u.SampleDate, u.SampleTime, u.UploadTime, u.SampleLocation, u.SheepBreed, u.Comments, u.ImageData,
                   m.nalePercentage, m.qufuPercentage, m.erciPercentage, m.bubblePercentage
            FROM Uploads u
            LEFT JOIN MLData m ON u.UploadName = m.UploadName";

                using (var command = new SqlCommand(query, _connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var upload = new UploadInfo
                        {
                            UploadName = reader.GetString(0),
                            SampleDate = reader.GetString(1),
                            SampleTime = reader.GetString(2),
                            UploadTime = reader.GetDateTime(3),
                            SampleLocation = reader.GetString(4),
                            SheepBreed = reader.GetString(5),
                            Comments = reader.IsDBNull(6) ? null : reader.GetString(6),
                        };

                        // Retrieve Image Data
                        if (!reader.IsDBNull(7))
                        {
                            byte[] imageData = (byte[])reader[7];
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                upload.ImageFile = Image.FromStream(ms);
                            }
                        }

                        var mlData = new MLData
                        {
                            nalePercentage = reader.IsDBNull(8) ? "0" : reader.GetString(8),
                            qufuPercentage = reader.IsDBNull(9) ? "0" : reader.GetString(9),
                            erciPercentage = reader.IsDBNull(10) ? "0" : reader.GetString(10),
                            bubblePercentage = reader.IsDBNull(11) ? "0" : reader.GetString(11)
                        };

                        uploadsWithML.Add((upload, mlData));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching uploads with ML data: {ex.Message}");
                throw;
            }

            return uploadsWithML;
        }

        // New Method for clearing DB data 
        public async Task ClearAllUploadsAsync()
        {
            try
            {
                await EnsureConnectionOpenAsync();

                using (var command = new SqlCommand("DELETE FROM Uploads; DELETE FROM MLData;", _connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                Console.WriteLine("All uploads cleared from the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing uploads: {ex.Message}");
                throw;
            }
        }

        private async Task EnsureConnectionOpenAsync()
        {
            if (_connection?.State != ConnectionState.Open)
            {
                MessageBox.Show("Opening new database connection...");
                _connection = new SqlConnection(_connectionString);
                await _connection.OpenAsync();
                MessageBox.Show($"Connection opened. State: {_connection.State}");
            }
        }

        private async Task CleanupAsync()
        {
            if (_connection != null)
            {
                MessageBox.Show("\nStarting connection cleanup...");
                if (_connection.State == ConnectionState.Open)
                {
                    MessageBox.Show("Closing open connection...");
                    await _connection.CloseAsync();
                    MessageBox.Show("Connection closed successfully.");
                }
                MessageBox.Show("Disposing connection...");
                await _connection.DisposeAsync();
                _connection = null;
                MessageBox.Show("Connection disposed.");
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
                            MessageBox.Show($"\nImage found: {imageName}");
                            MessageBox.Show("Image data is available (binary content not displayed)");
                        }
                        else
                        {
                            MessageBox.Show($"\nNo image found for {identifier}.png");
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
                            MessageBox.Show($"\nNo metadata found for {identifier}.txt");
                            return;
                        }

                        MessageBox.Show("\nMetadata from CSVDB:");
                        MessageBox.Show("===================");

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
                MessageBox.Show("No columns found in the metadata.");
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


            // Print separator
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                Console.Write(new string('-', columnWidths[i]) + "  ");
            }


            // Print data rows
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var value = row.IsNull(i) ? "NULL" : row[i]?.ToString() ?? "NULL";
                    Console.Write($"{value.PadRight(columnWidths[i] + 2)}");
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                MessageBox.Show("\nDisposing DatabaseQueries instance...");
                await CleanupAsync();
                _disposed = true;
                MessageBox.Show("DatabaseQueries instance disposed.");
            }
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                MessageBox.Show("\nDisposing DatabaseQueries instance (sync)...");
                CleanupAsync().GetAwaiter().GetResult();
                _disposed = true;
                MessageBox.Show("DatabaseQueries instance disposed (sync).");
            }
            GC.SuppressFinalize(this);
        }

        ~DBQueries()
        {
            Dispose();
        }


    }
}
    