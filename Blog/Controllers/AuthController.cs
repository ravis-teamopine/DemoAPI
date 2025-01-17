using Blog.Context;
using Blog.Models;
using Blog.Response;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly DapperContext _blogContext;

    public AuthController( JwtTokenService jwtTokenService, DapperContext blogContext)
    {
        _jwtTokenService = jwtTokenService;
        _blogContext = blogContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var connection = _blogContext.CreateConnection();

        var sql = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";

        // Pass parameters correctly
        var user = connection.QuerySingleOrDefault<User>(sql, new { Username = request.Username, Password = request.Password });

        if (user == null)
        {
            return BadRequest(ApiResponse<string>.Error("Invalid username or password"));
        }

        // Generate JWT token and return it
        var token = _jwtTokenService.GenerateToken(user.Username);
        return Ok(ApiResponse<string>.Success(token, "Login successful"));



    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes("AU2yDeCqX638cKszdEBj7WVn5Pgu9M4amhHvYTZLrGRwbfNFJk"))) // Replace with a secure salt
        {
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }
    }
}
