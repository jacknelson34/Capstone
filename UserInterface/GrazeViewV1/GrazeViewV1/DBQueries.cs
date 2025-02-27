using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;

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

        // Pull CSV Data from DB - Works
        public async Task<List<Dictionary<string, object>>> GetCSVDBDataAsync()
        {
            var results = new List<Dictionary<string, object>>();

            try
            {
                await EnsureConnectionOpenAsync();
                string query = "SELECT * FROM CSVDB";

                using (var command = new SqlCommand(query, _connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        results.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching CSVDB data: {ex.Message}");
            }

            return results;
        }

        // Pull Image Data From DB - Works
        public Bitmap RetrieveImageFromDB(int imageIndex)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Debugging: Show all available images before searching
                    string availableImages = "Available Images in DB:\n";
                    using (SqlCommand listCmd = new SqlCommand("SELECT ImageID, ImageName FROM Images", conn))
                    using (SqlDataReader reader = listCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            availableImages += $"{reader["ImageID"]}: {reader["ImageName"]}\n";
                        }
                    }
                    MessageBox.Show(availableImages, "Database Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Fetch image using index
                    string query = "SELECT ImageData FROM Images ORDER BY ImageID OFFSET @Index ROWS FETCH NEXT 1 ROWS ONLY";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Index", imageIndex);
                        var result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            MessageBox.Show($"No image found at index {imageIndex}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }

                        // Convert byte array to Bitmap
                        byte[] imageBytes = (byte[])result;
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            return new Bitmap(ms);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving image: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Push Image Data to DB - Works
        public void UploadImageToDB(string imagePath)
        {

            try
            {
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show($"Error: File not found - {imagePath}");
                    return;
                }

                string imageName = Path.GetFileName(imagePath);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Check if the image already exists in the database
                    string checkQuery = "SELECT COUNT(*) FROM Images WHERE ImageName = @ImageName";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@ImageName", imageName);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show($"Skipped: Image '{imageName}' already exists in the database.");
                            return;
                        }
                    }

                    // Convert the image to a byte array
                    byte[] imageBytes = File.ReadAllBytes(imagePath);

                    // Insert image into the database
                    string insertQuery = "INSERT INTO Images (ImageName, ImageData) VALUES (@ImageName, @ImageData)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@ImageName", imageName);
                        insertCmd.Parameters.AddWithValue("@ImageData", imageBytes);
                        insertCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Uploaded: {imageName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
            }
        }

        // Push CSV Data to DB - TODO


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

                MessageBox.Show("All uploads cleared from the database.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing uploads: {ex.Message}");
                throw;
            }
        }

        private async Task EnsureConnectionOpenAsync()
        {
            if (_connection?.State != ConnectionState.Open)
            {
                //MessageBox.Show("Opening new database connection...");
                _connection = new SqlConnection(_connectionString);
                await _connection.OpenAsync();
                //MessageBox.Show($"Connection opened. State: {_connection.State}");
            }
        }

        private async Task CleanupAsync()
        {
            if (_connection != null)
            {
                //MessageBox.Show("\nStarting connection cleanup...");
                if (_connection.State == ConnectionState.Open)
                {
                    //MessageBox.Show("Closing open connection...");
                    await _connection.CloseAsync();
                    //MessageBox.Show("Connection closed successfully.");
                }
                //MessageBox.Show("Disposing connection...");
                await _connection.DisposeAsync();
                _connection = null;
                //MessageBox.Show("Connection disposed.");
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
    