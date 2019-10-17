using ChrisKaczor.HomeMonitor.Weather.Models;
using ChrisKaczor.HomeMonitor.Weather.Service.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

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
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = _configuration["Weather:Database:Host"],
                UserID = _configuration["Weather:Database:User"],
                Password = _configuration["Weather:Database:Password"],
                InitialCatalog = "master"
            };

            using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);

            var command = new SqlCommand { Connection = connection };

            connection.Open();

            // Check to see if the database exists
            command.CommandText = $"SELECT CAST(1 as bit) from sys.databases WHERE name='{_configuration["Weather:Database:Name"]}'";
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

        private SqlConnection CreateConnection()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = _configuration["Weather:Database:Host"],
                UserID = _configuration["Weather:Database:User"],
                Password = _configuration["Weather:Database:Password"],
                InitialCatalog = _configuration["Weather:Database:Name"]
            };

            var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        }

        public void StoreWeatherData(WeatherMessage weatherMessage)
        {
            using var connection = CreateConnection();

            var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Weather.Service.Data.Resources.CreateReading.sql");

            connection.Query(query, weatherMessage);
        }

        public async Task<WeatherReading> GetRecentReading()
        {
            await using var connection = CreateConnection();

            var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Weather.Service.Data.Resources.GetRecentReading.sql");

            return await connection.QueryFirstOrDefaultAsync<WeatherReading>(query);
        }

        public async Task<IEnumerable<WeatherReading>> GetReadingHistory(DateTimeOffset start, DateTimeOffset end)
        {
            await using var connection = CreateConnection();

            var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Weather.Service.Data.Resources.GetReadingHistory.sql");

            return await connection.QueryAsync<WeatherReading>(query, new { Start = start, End = end });
        }

        public async Task<IEnumerable<WeatherValue>> GetReadingValueHistory(WeatherValueType weatherValueType, DateTimeOffset start, DateTimeOffset end)
        {
            await using var connection = CreateConnection();

            var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Weather.Service.Data.Resources.GetReadingValueHistory.sql");

            query = query.Replace("@Value", weatherValueType.ToString());

            return await connection.QueryAsync<WeatherValue>(query, new { Start = start, End = end });
        }

        public async Task<IEnumerable<WeatherValueGrouped>> GetReadingValueHistoryGrouped(WeatherValueType weatherValueType, DateTimeOffset start, DateTimeOffset end, int bucketMinutes)
        {
            await using var connection = CreateConnection();

            var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Weather.Service.Data.Resources.GetReadingValueHistoryGrouped.sql");

            switch (weatherValueType)
            {
                case WeatherValueType.LightLevel:
                    query = query.Replace("@Value", "LightLevel / BatteryLevel");
                    break;
                default:
                    query = query.Replace("@Value", weatherValueType.ToString());
                    break;
            }

            return await connection.QueryAsync<WeatherValueGrouped>(query, new { Start = start, End = end, BucketMinutes = bucketMinutes });
        }

        public async Task<IEnumerable<WeatherReadingGrouped>> GetReadingHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes)
        {
            await using var connection = CreateConnection();

            var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Weather.Service.Data.Resources.GetReadingHistoryGrouped.sql");

            return await connection.QueryAsync<WeatherReadingGrouped>(query, new { Start = start, End = end, BucketMinutes = bucketMinutes });
        }

        public async Task<IEnumerable<WindHistoryGrouped>> GetWindHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes)
        {
            await using var connection = CreateConnection();

            var query = ResourceReader.GetString("ChrisKaczor.HomeMonitor.Weather.Service.Data.Resources.GetWindHistoryGrouped.sql");

            return await connection.QueryAsync<WindHistoryGrouped>(query, new { Start = start, End = end, BucketMinutes = bucketMinutes });
        }
    }
}