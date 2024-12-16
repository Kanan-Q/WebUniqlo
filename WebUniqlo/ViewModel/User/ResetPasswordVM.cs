using System.ComponentModel.DataAnnotations;

namespace WebUniqlo.ViewModel.User
{
    public class ResetPasswordVM
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password yanlisdir!")]
        public string ConfirmPassword { get; set; }
    }
}
