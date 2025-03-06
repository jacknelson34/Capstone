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
                    //MessageBox.Show(availableImages, "Database Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        public int UploadImageToDB(string imagePath)
        {

            try
            {
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show($"Error: File not found - {imagePath}");
                    return 0;
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
                            DialogResult doubleUploadCheck = MessageBox.Show("This image has already been uploaded.  Would you like to upload again?",
                                            "Duplicate Image",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question 
                                            );


                            if (doubleUploadCheck == DialogResult.No) 
                            {
                                // Cancel Upload
                                return 1;    
                            }

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

                    //MessageBox.Show($"Uploaded: {imageName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
                return 0;
            }

            // 0 - Error
            // 1 - Upload Cancelled by User
            // 2 - Continue with Upload
            return 2;
        }

        // Push CSV Data to DB - Works
        public bool UploadCSVToDB(List<string> csvData)
        {
            bool success = false;

            string insertQuery = @"
        INSERT INTO CSVDB (SourceFile, QufuPercent, NalePercent, ErciPercent, 
        AirBubblePercent, DateSampleTaken, TimeSampleTaken, UploadDate, UploadTime, 
        SampleLocation, SheepBreed, Comments) 
        VALUES (@SourceFile, @QufuPercent, @NalePercent, @ErciPercent, @AirBubblePercent, 
        @DateSampleTaken, @TimeSampleTaken, @UploadDate, @UploadTime, @SampleLocation, 
        @SheepBreed, @Comments);";

            try
            {
                EnsureConnectionOpen(); // Assuming this method ensures the connection is open

                using (var command = new SqlCommand(insertQuery, _connection))
                {
                    // Assuming csvData contains values in the exact order of the database fields
                    command.Parameters.AddWithValue("@SourceFile", csvData[0]);
                    command.Parameters.AddWithValue("@QufuPercent", csvData[2]);
                    command.Parameters.AddWithValue("@NalePercent", csvData[1]);
                    command.Parameters.AddWithValue("@ErciPercent", csvData[3]);
                    command.Parameters.AddWithValue("@AirBubblePercent", csvData[4]);
                    command.Parameters.AddWithValue("@DateSampleTaken", csvData[5]);
                    command.Parameters.AddWithValue("@TimeSampleTaken", csvData[6]);
                    command.Parameters.AddWithValue("@UploadDate", csvData[7]);
                    command.Parameters.AddWithValue("@UploadTime", csvData[8]);
                    command.Parameters.AddWithValue("@SampleLocation", csvData[9]);
                    command.Parameters.AddWithValue("@SheepBreed", csvData[10]);
                    command.Parameters.AddWithValue("@Comments", csvData[11]);

                    int rowsAffected = command.ExecuteNonQuery();
                    success = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting CSV data: {ex.Message}");
            }

            return success;
        }

        // Check DB Connection State
        private void EnsureConnectionOpen()
        {
            if (_connection == null)
            {
                // Initialize the connection with a valid connection string
                string connectionString = "Server=sqldatabase404.database.windows.net;Database=404ImageDBsql;User Id=sql404admin;Password=sheepstool404();TrustServerCertificate=False;MultipleActiveResultSets=True;";
                _connection = new SqlConnection(connectionString);
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        // New Method for clearing DB data - Works
        public async Task ClearAllUploadsAsync()
        {
            try
            {
                await EnsureConnectionOpenAsync();

                // Start a transaction to ensure both deletions occur together
                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        // Delete images from the Images table (modify table name if needed)
                        string deleteImagesQuery = "TRUNCATE TABLE Images";

                        using (var imageCommand = new SqlCommand(deleteImagesQuery, _connection, transaction))
                        {
                            int imagesDeleted = await imageCommand.ExecuteNonQueryAsync();
                            Console.WriteLine($"Deleted {imagesDeleted} images.");
                        }

                        // Delete upload records from CSVDB 
                        string deleteUploadsQuery = "DELETE FROM CSVDB";

                        using (var uploadCommand = new SqlCommand(deleteUploadsQuery, _connection, transaction))
                        {
                            int uploadsDeleted = await uploadCommand.ExecuteNonQueryAsync();
                            Console.WriteLine($"Deleted {uploadsDeleted} uploads.");
                        }

                        // Commit transaction if both deletions succeed
                        transaction.Commit();
                        MessageBox.Show("All uploads and images successfully deleted.", "Clear Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if an error occurs
                        transaction.Rollback();
                        MessageBox.Show($"Error clearing database: {ex.Message}", "Clear Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing database: {ex.Message}", "Clear Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Query for DataLibraryExpandedView
        public async Task<Dictionary<string, object>> GetRowByIndexAsync(int rowIndex)
        {
            Dictionary<string, object> rowData = new Dictionary<string, object>();

            try
            {
                await EnsureConnectionOpenAsync();

                string query = "SELECT * FROM CSVDB WHERE ID = @RowIndex";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@RowIndex", rowIndex);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                rowData[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving row data: {ex.Message}");
            }

            return rowData;
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

        public void Dispose()
        {
            if (!_disposed)
            {
                //MessageBox.Show("\nDisposing DatabaseQueries instance (sync)...");
                CleanupAsync().GetAwaiter().GetResult();
                _disposed = true;
                //MessageBox.Show("DatabaseQueries instance disposed (sync).");
            }
            GC.SuppressFinalize(this);
        }

        ~DBQueries()
        {
            Dispose();
        }


    }
}
    