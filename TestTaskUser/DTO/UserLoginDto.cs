using System.ComponentModel.DataAnnotations;
using TestTaskUser.Helpers;

namespace TestTaskUser.DTO
{
    /// <summary>
    /// Информация о пользователе (для входа в учетную запись)
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// Электронная почта
        /// </summary>
        [Required]
        [EmailAddress]
        [EmailExists]
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
