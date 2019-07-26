using ChrisKaczor.HomeMonitor.Weather.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Data
{
    public class Database
    {
        private readonly IConfiguration _configuration;

        public Database(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void EnsureDatabase()
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = _configuration["Weather:Database:Host"],
                Username = _configuration["Weather:Database:User"],
                Password = _configuration["Weather:Database:Password"],
                Database = "postgres"
            };

            using (var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString))
            {
                var command = new NpgsqlCommand { Connection = connection };

                connection.Open();

                // Check to see if the database exists
                command.CommandText = $"SELECT TRUE from pg_database WHERE datname='{_configuration["Weather:Database:Name"]}'";
                var databaseExists = (bool?)command.ExecuteScalar();

                // Create database if needed
                if (!databaseExists.GetValueOrDefault(false))
                {
                    command.CommandText = $"CREATE DATABASE {_configuration["Weather:Database:Name"]}";
                    command.ExecuteNonQuery();
                }

                // Switch to the database now that we're sure it exists
                connection.ChangeDatabase(_configuration["Weather:Database:Name"]);

                var schema = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Weather.Service.Data.Resources.Schema.sql");

                // Make sure the database is up to date
                command.CommandText = schema;
                command.ExecuteNonQuery();
            }
        }

        private NpgsqlConnection CreateConnection()
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = _configuration["Weather:Database:Host"],
                Username = _configuration["Weather:Database:User"],
                Password = _configuration["Weather:Database:Password"],
                Database = _configuration["Weather:Database:Name"]
            };

            var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        }

        public void StoreWeatherData(WeatherMessage weatherMessage)
        {
            using (var connection = CreateConnection())
            {
                var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Weather.Service.Data.Resources.CreateReading.sql");

                connection.Execute(query, weatherMessage);
            }
        }
    }
}