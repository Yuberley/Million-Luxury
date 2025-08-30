namespace MillionLuxury.Infrastructure.Database;

#region Usings
using Microsoft.Data.SqlClient;
using MillionLuxury.Application.Common.Abstractions.Data;
using System.Data; 
#endregion

internal sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}