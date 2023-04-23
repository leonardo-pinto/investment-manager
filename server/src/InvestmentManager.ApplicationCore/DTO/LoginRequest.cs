using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.DTO
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "User name can't be blank")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
