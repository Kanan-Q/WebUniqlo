using System.ComponentModel.DataAnnotations;

namespace WebUniqlo.ViewModel.User
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Write Your Email.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
