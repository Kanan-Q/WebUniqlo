using System.ComponentModel.DataAnnotations;

namespace WebUniqlo.ViewModel.User
{
    public class LoginVM
    {
        [Required]
        public string UsernameOrEmail {  get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool RememberMe {  get; set; }
    }
}
