using ChrisKaczor.HomeMonitor.Environment.Service.Models;
using ChrisKaczor.HomeMonitor.Environment.Service.Models.Indoor;
using Dapper;
using DbUp;
using Npgsql;
using System.Reflection;

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

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Environment.Service.Data.Queries.CreateReading.psql");

        await connection.QueryAsync(query, message);
    }

    public async Task<IEnumerable<Readings>> GetRecentReadings()
    {
        await using var connection = CreateConnection();

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Environment.Service.Data.Queries.GetRecentReadings.psql");

        return await connection.QueryAsync<Readings>(query).ConfigureAwait(false);
    }

    public async Task<IEnumerable<ReadingsGrouped>> GetReadingsHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes)
    {
        await using var connection = CreateConnection();

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Environment.Service.Data.Queries.GetReadingsHistoryGrouped.psql");

        query = query.Replace("@BucketMinutes", bucketMinutes.ToString());

        return await connection.QueryAsync<ReadingsGrouped>(query, new { Start = start, End = end, BucketMinutes = bucketMinutes }).ConfigureAwait(false);
    }

    public async Task<IEnumerable<ReadingsAggregate>> GetReadingsAggregate(DateTimeOffset start, DateTimeOffset end)
    {
        await using var connection = CreateConnection();

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Environment.Service.Data.Queries.GetReadingsAggregate.psql");

        return await connection.QueryAsync<ReadingsAggregate>(query, new { Start = start, End = end }).ConfigureAwait(false);
    }
}