using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTO
{
    public class LoginUser
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [RegularExpression(@"^.{3,}@gmail\.com$", ErrorMessage = "Email address must be from @gmail.com domain")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^.{6,20}$", ErrorMessage = "Password must be between 6 and 20 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
