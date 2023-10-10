using TestTaskUser.Enums;

namespace TestTaskUser.Helpers
{
    /// <summary>
    /// Параметры сортировки
    /// </summary>
    public class Sorting
    {
        /// <summary>
        /// Название поля для сортировки
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public SortingOrder Order { get; set; }
    }
}
