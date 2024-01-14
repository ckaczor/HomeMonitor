using System.Reflection;
using Dapper;
using DbUp;
using Microsoft.Data.SqlClient;

namespace ChrisKaczor.HomeMonitor.Environment.Service.Data;

public class Database(IConfiguration configuration)
{
    private string GetConnectionString()
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = $"{configuration["Environment:Database:Host"]},{configuration["Environment:Database:Port"]}",
            UserID = configuration["Environment:Database:User"],
            Password = configuration["Environment:Database:Password"],
            InitialCatalog = configuration["Environment:Database:Name"],
            TrustServerCertificate = bool.Parse(configuration["Environment:Database:TrustServerCertificate"] ?? "false")
        };

        return connectionStringBuilder.ConnectionString;
    }

    public void EnsureDatabase()
    {
        var connectionString = GetConnectionString();

        DbUp.EnsureDatabase.For.SqlDatabase(connectionString);

        var upgradeEngine = DeployChanges.To.SqlDatabase(connectionString).WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.Contains(".Schema.")).LogToConsole().Build();

        upgradeEngine.PerformUpgrade();
    }

    private SqlConnection CreateConnection()
    {
        var connection = new SqlConnection(GetConnectionString());
        connection.Open();

        return connection;
    }

    public async Task StoreMessageAsync(Message message)
    {
        await using var connection = CreateConnection();

        var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Environment.Service.Data.Queries.CreateReading.sql");

        await connection.QueryAsync(query, message);
    }
}