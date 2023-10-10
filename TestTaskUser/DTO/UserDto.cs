using TestTaskUser.Models;

namespace TestTaskUser.DTO
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Список ролей
        /// </summary>
        public List<RoleDto> Roles { get; set; }
    }
}
