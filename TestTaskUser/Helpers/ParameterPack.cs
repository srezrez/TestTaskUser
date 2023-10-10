namespace TestTaskUser.Helpers
{
    /// <summary>
    /// Параметры отображения
    /// </summary>
    public class ParameterPack
    {
        /// <summary>
        /// Сортировка
        /// </summary>
        public Sorting? Sorting { get; set; }

        /// <summary>
        /// Поисковое значение
        /// </summary>
        public string SearchTerm { get; set; } = string.Empty;

        /// <summary>
        /// Пагинация
        /// </summary>
        public Pagination? Pagination { get; set; }
    }
}
