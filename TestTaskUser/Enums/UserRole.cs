namespace TestTaskUser.Enums
{
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public enum UserRole : int
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        User = 0,

        /// <summary>
        /// Администратор
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Поддержка
        /// </summary>
        Support = 2,

        /// <summary>
        /// Супер администратор
        /// </summary>
        SuperAdmin = 3,
    }
}
