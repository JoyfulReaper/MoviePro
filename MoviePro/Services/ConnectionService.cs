using Microsoft.Extensions.Configuration;
using System;

namespace MoviePro.Services
{
    public class ConnectionService
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            // Since we are using SQL Server and real hosting we won't use this anyway.
            return "BAD_CONNECTION_STRING";
            //var databaseUri = new Uri(databaseUrl);
            //var userInfo = databaseUri.UserInfo.Split(':');
            //var builder = new NpgsqlConnectionStringBuilder
            //{
            //    Host = databaseUri.Host,
            //    Port = databaseUri.Port,
            //    Username = userInfo[0],
            //    Password = userInfo[1],
            //    Database = databaseUri.LocalPath.TrimStart('/'),
            //    SslMode = SslMode.Require,
            //    TrustServerCertificate = true
            //};
            //return builder.ToString();
        }
    }
}
