using System;
using System.Threading.Tasks;

namespace SqlServerConnector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("SQL Server Connection Test Application");
            Console.WriteLine("=====================================");

            // Create connection settings with provided credentials
            var settings = new ConnectionSettings(
                server: "sqldatabase404.database.windows.net",
                database: "404ImageDBsql",
                username: "sql404admin",
                password: "sheepstool404()"
            );

            // Create database connection instance
            var dbConnection = new DatabaseConnection(settings);

            try
            {
                // Test the connection
                bool isConnected = await dbConnection.TestConnectionAsync();

                if (isConnected)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nConnection successful!");
                    Console.ResetColor();

                    // Create database queries instance with proper disposal
                    await using (var dbQueries = new DatabaseQueries(dbConnection.ConnectionString))
                    {
                        Console.WriteLine("\nQuerying database...");

                        // Fetch and display identifiers
                        var identifiers = await dbQueries.FetchIdentifiersAsync();
                        Console.WriteLine("\nFound common identifiers:");
                        foreach (var identifier in identifiers)
                        {
                            Console.WriteLine($"- {identifier}");
                        }

                        // If we have any identifiers, display data for the first one
                        if (identifiers.Count > 0)
                        {
                            Console.WriteLine($"\nDisplaying data for first identifier: {identifiers[0]}");
                            await dbQueries.DisplayDataForIdentifierAsync(identifiers[0]);
                        }

                        Console.WriteLine("\nClosing database queries and cleaning up resources...");
                    } // DatabaseQueries is automatically disposed here

                    Console.WriteLine("Database resources cleaned up successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nOperation failed: {ex.Message}");
                Console.ResetColor();
            }

            // Give time to see the output and ensure cleanup completes
            Console.WriteLine("\nPress any key to exit...");
            await Task.Delay(3000); // Increased delay to 3 seconds to ensure cleanup messages are visible
        }
    }
}