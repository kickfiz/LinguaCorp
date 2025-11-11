using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinguaCorp.API.Controllers;

/// <summary>
/// Authentication controller for JWT token generation
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Login endpoint to obtain JWT token
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token if credentials are valid</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for username: {Username}", request.Username);

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            _logger.LogWarning("Login attempt with empty credentials");
            return BadRequest(new { message = "Username and password are required." });
        }

        // Validate credentials from appsettings
        var adminUsername = _configuration["AdminCredentials:Username"];
        var adminPassword = _configuration["AdminCredentials:Password"];

        if (request.Username != adminUsername || request.Password != adminPassword)
        {
            _logger.LogWarning("Invalid login attempt for username: {Username}", request.Username);
            return Unauthorized(new { message = "Invalid credentials." });
        }

        // Generate JWT token
        var token = GenerateJwtToken(request.Username);

        _logger.LogInformation("User {Username} logged in successfully", request.Username);

        return Ok(new LoginResponse
        {
            Token = token,
            ExpiresIn = 86400 // 24 hours in seconds
        });
    }

    private string GenerateJwtToken(string username)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationHours = int.Parse(jwtSettings["ExpirationHours"]);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

/// <summary>
/// Login request model
/// </summary>
public class LoginRequest
{
    /// <summary>Username</summary>
    public string Username { get; set; }

    /// <summary>Password</summary>
    public string Password { get; set; }
}

/// <summary>
/// Login response model
/// </summary>
public class LoginResponse
{
    /// <summary>JWT access token</summary>
    public string Token { get; set; }

    /// <summary>Token expiration time in seconds</summary>
    public int ExpiresIn { get; set; }
}
