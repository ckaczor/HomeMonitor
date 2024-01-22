using ChrisKaczor.HomeMonitor.Power.Service.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Power.Service.Data;

public class Database(IConfiguration configuration)
{
    public void EnsureDatabase()
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = $"{configuration["Power:Database:Host"]},{configuration["Power:Database:Port"]}",
            UserID = configuration["Power:Database:User"],
            Password = configuration["Power:Database:Password"],
            InitialCatalog = "master",
            TrustServerCertificate = bool.Parse(configuration["Power:Database:TrustServerCertificate"] ?? "false")
        };

        using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);

        var command = new SqlCommand { Connection = connection };

        connection.Open();

        // Check to see if the database exists
        command.CommandText = $"SELECT CAST(1 as bit) from sys.databases WHERE name='{configuration["Power:Database:Name"]}'";
        var databaseExists = (bool?)command.ExecuteScalar();

        // Create database if needed
        if (!(databaseExists ?? false))
        {
            command.CommandText = $"CREATE DATABASE {configuration["Power:Database:Name"]}";
            command.ExecuteNonQuery();
        }

        // Switch to the database now that we're sure it exists
        connection.ChangeDatabase(configuration["Power:Database:Name"]!);

        var schema = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Power.Service.Data.Resources.Schema.sql");

        // Make sure the database is up-to-date
        command.CommandText = schema;
        command.ExecuteNonQuery();
    }

    private SqlConnection CreateConnection()
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = $"{configuration["Power:Database:Host"]},{configuration["Power:Database:Port"]}",
            UserID = configuration["Power:Database:User"],
            Password = configuration["Power:Database:Password"],
            InitialCatalog = configuration["Power:Database:Name"],
            TrustServerCertificate = bool.Parse(configuration["Power:Database:TrustServerCertificate"] ?? "false")
        };

        var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
        connection.Open();

        return connection;
    }

    public void StorePowerData(PowerStatus powerStatus)
    {
        using var connection = CreateConnection();

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Power.Service.Data.Resources.CreateStatus.sql");

        connection.Query(query, powerStatus);
    }

    public async Task<PowerStatus> GetRecentStatus()
    {
        await using var connection = CreateConnection();

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Power.Service.Data.Resources.GetRecentStatus.sql");

        return await connection.QueryFirstOrDefaultAsync<PowerStatus>(query);
    }

    public async Task<IEnumerable<PowerStatusGrouped>> GetStatusHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes)
    {
        await using var connection = CreateConnection();

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Power.Service.Data.Resources.GetStatusHistoryGrouped.sql");

        return await connection.QueryAsync<PowerStatusGrouped>(query, new { Start = start, End = end, BucketMinutes = bucketMinutes });
    }
}