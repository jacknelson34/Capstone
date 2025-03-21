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
using OpenCvSharp.Flann;
using System.Data.Odbc;
using System.Diagnostics;

namespace GrazeViewV1
{
    // Class to connect the UI to the DB - TODO
    public class DBQueries
    {

        private readonly string _connectionString;
        private OdbcConnection? _connection;
        private bool _disposed;

        public DBQueries(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _connection = new OdbcConnection(_connectionString);
        }

        // Pull CSV Data from DB - Works
        public async Task<List<Dictionary<string, object>>> GetCSVDBDataAsync()
        {
            var results = new List<Dictionary<string, object>>();

            try
            {
                await EnsureConnectionOpenAsync();
                string query = @"
                SELECT 
                    ID,
                    SourceFile,
                    QufuPercent,
                    NalePercent,
                    ErciPercent,
                    AirBubblePercent,
                    DateSampleTaken,
                    CAST(TimeSampleTaken AS VARCHAR(8)) AS TimeSampleTaken, 
                    UploadDate,
                    CAST(UploadTime AS VARCHAR(8)) AS UploadTime,
                    SampleLocation,
                    SheepBreed,
                    Comments
                FROM CSVDB";


                using (OdbcCommand command = new OdbcCommand(query, _connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
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

        // Pull image from DB - take 2
        public async Task<Bitmap> RetrieveImageFromDB(int imageIndex)
        {

            try
            {
                using (OdbcConnection conn = new OdbcConnection(_connectionString))
                {
                    await conn.OpenAsync(); // Ensure async database connection

                    // Use TOP 1 for better performance (avoids OFFSET issues)
                    string query = "SELECT ImageData FROM Images WHERE ImageID = ?";

                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", imageIndex);
                        cmd.CommandTimeout = 120; // Increase timeout for large images

                        using (OdbcDataReader reader = (OdbcDataReader) cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show($"No image found at index {imageIndex}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return null;
                            }

                            // Read image stream instead of loading full byte array
                            using (MemoryStream ms = new MemoryStream())
                            {
                                long length = reader.GetBytes(0, 0, null, 0, 0);
                                byte[] buffer = new byte[length];
                                reader.GetBytes(0, 0, buffer, 0, (int)length);

                                //MessageBox.Show("Returning image at index: " + imageIndex);

                                return new Bitmap(new MemoryStream(buffer));
                            }
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

        // Check db for duplicate upload - DONE
        public async Task<int> DuplicateImageCheck(string imagePath)
        {
            // 0 - Error
            // 1 - Upload Cancelled by User
            // 2 - Continue with Upload

            try
            {
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show($"Error: File not found - {imagePath}");
                    return 0;
                }

                string imageName = Path.GetFileName(imagePath);

                using (OdbcConnection conn = new OdbcConnection(_connectionString)) 
                {
                    await conn.OpenAsync();

                    // Check if the image already exists in the database
                    string checkQuery = "SELECT COUNT(*) FROM Images WHERE ImageName = ?";
                    using (OdbcCommand checkCmd = new OdbcCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.Add(new OdbcParameter("?", OdbcType.VarChar, 255) { Value = imageName });
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            DialogResult doubleUploadCheck = MessageBox.Show(
                                            "This image has already been uploaded.  Would you like to upload again?",
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
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
                return 0;
            }
            return 2;
        }

        // Push Image Data to DB - Need to adjust time it takes
        public async Task UploadImageToDB(string imagePath)
        {

            try
            {
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show($"Error: File not found - {imagePath}");
                }

                string imageName = Path.GetFileName(imagePath);

                using (OdbcConnection conn = new OdbcConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Convert the image to a byte array
                    byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

                    // Insert image into the database
                    string insertQuery = "INSERT INTO Images (ImageName, ImageData) VALUES (?, ?)";
                    using (OdbcCommand insertCmd = new OdbcCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.Add("?", OdbcType.VarChar, 255).Value = imageName;
                        insertCmd.Parameters.Add("?", OdbcType.Image, imageBytes.Length).Value = imageBytes;

                        await insertCmd.ExecuteNonQueryAsync();
                    }

                    //MessageBox.Show($"Uploaded: {imageName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
            }

        }

        // Push CSV Data to DB - Works
        public bool UploadCSVToDB(List<string> csvData)
        {
            bool success = false;

            string insertQuery = @"
        INSERT INTO CSVDB (SourceFile, QufuPercent, NalePercent, ErciPercent, 
        AirBubblePercent, DateSampleTaken, TimeSampleTaken, UploadDate, UploadTime, 
        SampleLocation, SheepBreed, Comments) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);";

            try
            {
                EnsureConnectionOpen(); // Assuming this method ensures the connection is open

                using (OdbcCommand command = new OdbcCommand(insertQuery, _connection))
                {
                    // Assuming csvData contains values in the exact order of the database fields
                    command.Parameters.AddWithValue("?", csvData[0]);                   // SourceFile
                    command.Parameters.AddWithValue("?", csvData[1]);                   // Qufu 
                    command.Parameters.AddWithValue("?", csvData[2]);                   // Nale
                    command.Parameters.AddWithValue("?", csvData[3]);                   // Erci
                    command.Parameters.AddWithValue("?", csvData[4]);                   // Air bubble
                    command.Parameters.AddWithValue("?", csvData[5]);                   // Sample Date
                    command.Parameters.AddWithValue("?", csvData[6]);                   // Sample Time
                    command.Parameters.AddWithValue("?", csvData[7]);                   // Upload Date
                    command.Parameters.AddWithValue("?", csvData[8]);                   // Upload Time
                    command.Parameters.AddWithValue("?", csvData[9]);                   // Location
                    command.Parameters.AddWithValue("?", csvData[10]);                  // Sheep Breed
                    command.Parameters.AddWithValue("?", csvData[11]);                  // Comments

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
                string connectionString = "Driver={ODBC Driver 18 for SQL Server};Server=sqldatabase404.database.windows.net;Database=404ImageDBsql;Uid=sql404admin;Pwd=sheepstool404();TrustServerCertificate=no;MultipleActiveResultSets=True;";
                _connection = new OdbcConnection(connectionString);
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

                // Start transaction for safety
                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        // DELETE instead of TRUNCATE for better ODBC compatibility
                        string deleteImagesQuery = "SET NOCOUNT ON; DELETE FROM Images; DBCC CHECKIDENT ('Images', RESEED, 0);";

                        using (var imageCommand = new OdbcCommand(deleteImagesQuery, _connection, transaction))
                        {
                            int imagesDeleted = await imageCommand.ExecuteNonQueryAsync();
                            //MessageBox.Show($"Deleted {imagesDeleted} images.");
                        }

                        // Ensure CSVDB records are fully reset
                        string deleteUploadsQuery = "SET NOCOUNT ON; DELETE FROM CSVDB; DBCC CHECKIDENT ('CSVDB', RESEED, 0);";

                        using (var uploadCommand = new OdbcCommand(deleteUploadsQuery, _connection, transaction))
                        {
                            int uploadsDeleted = await uploadCommand.ExecuteNonQueryAsync();
                            //MessageBox.Show($"Deleted {uploadsDeleted} uploads.");
                        }

                        // Commit transaction to apply changes
                        transaction.Commit();
                        MessageBox.Show("All uploads and images successfully deleted.", "Clear Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction on failure
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
        public async Task<Dictionary<string, object>> GetRowByIndexAsync(int index)
        {
            index--;
            try
            {
                var result = new Dictionary<string, object>();

                using (OdbcConnection conn = new OdbcConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Check if index exists before querying
                    // Fetch row data while ensuring TIME values are formatted properly
                    string checkQuery = @"
                        SELECT 
                            ID, 
                            SourceFile, 
                            QufuPercent, 
                            NalePercent, 
                            ErciPercent, 
                            AirBubblePercent, 
                            DateSampleTaken, 
                            CONVERT(VARCHAR, TimeSampleTaken, 108) AS TimeSampleTaken, -- Fix TIME issue
                            UploadDate, 
                            CONVERT(VARCHAR, UploadTime, 108) AS UploadTime, -- Fix TIME issue
                            SampleLocation, 
                            SheepBreed, 
                            Comments
                        FROM CSVDB
                        WHERE ID = ?;"; // ODBC uses '?' for parameters
                    using (OdbcCommand checkCmd = new OdbcCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("?", index);
                        int count = (int)await checkCmd.ExecuteScalarAsync();
                        if (count == 0)
                        {
                            MessageBox.Show($"No row found for index {index}.", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return null;
                        }
                    }

                    // Fetch row data while ensuring TIME values are formatted properly
                    string query = @"
                        SELECT 
                            ID, 
                            SourceFile, 
                            QufuPercent, 
                            NalePercent, 
                            ErciPercent, 
                            AirBubblePercent, 
                            DateSampleTaken, 
                            CONVERT(VARCHAR, TimeSampleTaken, 108) AS TimeSampleTaken, -- Fix TIME issue
                            UploadDate, 
                            CONVERT(VARCHAR, UploadTime, 108) AS UploadTime, -- Fix TIME issue
                            SampleLocation, 
                            SheepBreed, 
                            Comments
                        FROM CSVDB
                        WHERE ID = ?;"; 
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", index);
                        using (OdbcDataReader reader = (OdbcDataReader)await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync()) // Row found
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (!reader.IsDBNull(i))
                                    {
                                        object value = reader.GetValue(i);
                                        if (value is TimeSpan timeSpan)
                                        {
                                            result[reader.GetName(i)] = timeSpan.ToString(@"hh\:mm\:ss");
                                        }
                                        else
                                        {
                                            result[reader.GetName(i)] = value;
                                        }
                                    }
                                    else
                                    {
                                        result[reader.GetName(i)] = null;
                                    }
                                }
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving row data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


        private async Task EnsureConnectionOpenAsync()
        {
            if (_connection?.State != ConnectionState.Open)
            {
                //MessageBox.Show("Opening new database connection...");
                _connection = new OdbcConnection(_connectionString);
                await _connection.OpenAsync();
                //MessageBox.Show($"Connection opened. State: {_connection.State}");
            }
        }

        private async Task CleanupAsync()
        {
            if (_connection != null)
            {
                try
                {
                    // Ensure connection is open before closing
                    if (_connection.State != ConnectionState.Closed && _connection.State != ConnectionState.Broken)
                    {
                        await _connection.CloseAsync();
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error closing connection: {ex.Message}");
                }

                try
                {
                    // Ensure the connection handle exists before disposing
                    if (_connection != null)
                    {
                        await _connection.DisposeAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error disposing connection: {ex.Message}");
                }

                _connection = null; // Reset connection after cleanup
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
    