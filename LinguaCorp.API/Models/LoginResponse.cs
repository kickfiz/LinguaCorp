namespace LinguaCorp.API.Models
{
    /// <summary>
    /// Login response model containing authentication token
    /// </summary>
    public class LoginResponse
    {
        /// <summary>JWT access token</summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string Token { get; set; }

        /// <summary>Token expiration time in seconds</summary>
        /// <example>86400</example>
        public int ExpiresIn { get; set; }
    }
}
