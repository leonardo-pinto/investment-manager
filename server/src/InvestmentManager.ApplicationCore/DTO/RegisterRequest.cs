using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.DTO
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "User name can't be blank")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public required string ConfirmPassword { get; set; }
    }
}
