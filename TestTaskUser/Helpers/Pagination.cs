using System.ComponentModel.DataAnnotations;

namespace TestTaskUser.Helpers
{
    /// <summary>
    /// Параметры пагинации
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int PageSize { get; set; }
    }
}
