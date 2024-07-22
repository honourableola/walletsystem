using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public CurrencyType CurrencyType { get; set; }
    }
}
