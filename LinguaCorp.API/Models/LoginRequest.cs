using System.ComponentModel.DataAnnotations;

namespace LinguaCorp.API.Models
{
    /// <summary>
    /// Login request model for authentication
    /// </summary>
    public class LoginRequest
    {
        /// <summary>Username for authentication</summary>
        /// <example>admin</example>
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        /// <summary>Password for authentication</summary>
        /// <example>admin</example>
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
