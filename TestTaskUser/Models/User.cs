using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using TestTaskUser.Enums;

namespace TestTaskUser.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
