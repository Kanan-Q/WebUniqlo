using System.ComponentModel.DataAnnotations;

namespace WebUniqlo.ViewModel.User
{
    public class ForgotPasswordVM
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
