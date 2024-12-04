using System.ComponentModel.DataAnnotations;

namespace WebUniqlo.ViewModel.User
{
    public class UserCreateVM
    {
        [Required,MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,DataType(DataType.Password),Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
