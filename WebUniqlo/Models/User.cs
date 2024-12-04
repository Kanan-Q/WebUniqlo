using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace WebUniqlo.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        public string ImageURL { get; set; }
    }   
    
}
