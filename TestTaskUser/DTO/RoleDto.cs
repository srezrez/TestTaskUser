using TestTaskUser.Enums;
using TestTaskUser.Models;

namespace TestTaskUser.DTO
{
    /// <summary>
    /// Роль
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// Тип роли
        /// </summary>
        public UserRole UserRoleId { get; set; }

        /// <summary>
        /// Наименование роли
        /// </summary>
        public string Name { get; set; }
    }
}
