using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FantasyFootballService.Controllers;

public class PostgresControllerBase : ControllerBase
{
    protected readonly IConfiguration _config;
    
    protected PostgresControllerBase(IConfiguration config)
    {
        _config = config;
    }

    protected NpgsqlConnection OpenConnection()
    {
        var npgsqlConnection = new NpgsqlConnection(_config.GetConnectionString("PostgresConnection"));
        npgsqlConnection.Open();
        return npgsqlConnection;
    }

    protected void CloseConnection(NpgsqlConnection npgsqlConnection) => npgsqlConnection?.Close();
}