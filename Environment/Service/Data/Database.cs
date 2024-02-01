using ChrisKaczor.HomeMonitor.Environment.Service.Models;
using Dapper;
using DbUp;
using Npgsql;
using System.Reflection;
using ChrisKaczor.HomeMonitor.Environment.Service.Models.Indoor;

namespace ChrisKaczor.HomeMonitor.Environment.Service.Data;

public class Database(IConfiguration configuration)
{
    private string GetConnectionString()
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = configuration["Environment:Database:Host"],
            Port = configuration.GetValue<int>("Environment:Database:Port"),
            Username = configuration["Environment:Database:User"],
            Password = configuration["Environment:Database:Password"],
            Database = configuration["Environment:Database:Name"],
            TrustServerCertificate = bool.Parse(configuration["Environment:Database:TrustServerCertificate"] ?? "false")
        };

        return connectionStringBuilder.ConnectionString;
    }

    public void EnsureDatabase()
    {
        var connectionString = GetConnectionString();

        DbUp.EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgradeEngine = DeployChanges.To.PostgresqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.Contains(".Schema.")).LogToConsole().Build();

        upgradeEngine.PerformUpgrade();
    }

    private NpgsqlConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(GetConnectionString());
        connection.Open();

        return connection;
    }

    public async Task StoreMessageAsync(DeviceMessage message)
    {
        await using var connection = CreateConnection();

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Environment.Service.Data.Queries.CreateReading.sql");

        await connection.QueryAsync(query, message);
    }

    public async Task<IEnumerable<Readings>> GetRecentReadings()
    {
        await using var connection = CreateConnection();

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Environment.Service.Data.Queries.GetRecentReadings.sql");

        return await connection.QueryAsync<Readings>(query).ConfigureAwait(false);
    }
}