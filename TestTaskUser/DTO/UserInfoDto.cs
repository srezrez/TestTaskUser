using System.ComponentModel.DataAnnotations;
using TestTaskUser.Helpers;

namespace TestTaskUser.DTO
{
    /// <summary>
    /// Информация о пользователе (для добавления и изменения)
    /// </summary>
    public class UserInfoDto
    {

        /// <summary>
        /// Имя
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required field")]
        public string Name { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        [Required]
        [Range(1, 125, ErrorMessage = "Only positive number allowed in field age")]
        public int Age { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        [Required]
        [EmailAddress]
        [EmailUnique]
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
