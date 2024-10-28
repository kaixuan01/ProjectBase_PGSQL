using System;
using Npgsql;

class Program
{
    static void Main(string[] args)
    {
        // Local DB
        // var connectionString = "Host=localhost;Port=5432;Database=Project_Base;Username=postgres;Password=sa";

        // Server DB
        var connectionString = "Host=aws-0-ap-southeast-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.jnskzflwvkrjeafsajxj;Password=rotikosongtambah2telur/;";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connection successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        }
    }
}
