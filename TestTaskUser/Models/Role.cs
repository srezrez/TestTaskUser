using System.ComponentModel.DataAnnotations;
using TestTaskUser.Enums;

namespace TestTaskUser.Models
{
    public class Role
    {
        [Key]
        public UserRole UserRoleId { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
