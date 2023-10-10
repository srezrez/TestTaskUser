using System.ComponentModel.DataAnnotations;
using TestTaskUser.Data;

namespace TestTaskUser.Helpers
{
    public class EmailExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _dataContext = (DataContext)validationContext.GetService(typeof(DataContext));
            var user = _dataContext.Users.FirstOrDefault(u => u.Email == value.ToString());

            if (user == null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(string email)
        {
            return $"User with email {email} doesn't exist";
        }
    }
}
