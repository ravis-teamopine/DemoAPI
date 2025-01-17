using Blog.Context;
using Blog.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Authentication : ControllerBase
    {

        private readonly DapperContext _blogContext;

        public Authentication( DapperContext blogContext)
        {
            _blogContext = blogContext;
        }

        [Authorize]
        [HttpGet(Name = "Authentication")]
        public IActionResult Get()
        {
            using var connection = _blogContext.CreateConnection();
            var sql = "SELECT * FROM Users";
            List<User> users = connection.Query<User>(sql).ToList();
            return Ok(users);
        }

    }
}
