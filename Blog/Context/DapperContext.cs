using System.Data;
using MySqlConnector;

namespace Blog.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("DefaultConnection");
        }
        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}


