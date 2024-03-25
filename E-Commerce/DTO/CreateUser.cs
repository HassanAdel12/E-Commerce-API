using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTO
{
    public class CreateUser
    {
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [RegularExpression(@"^.{3,}@gmail\.com$", ErrorMessage = "Email address must be from @gmail.com domain")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^.{6,20}$", ErrorMessage = "Password must be between 6 and 20 characters long")]
        [DataType(DataType.Password)] 
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^.{6,20}$", ErrorMessage = "Password must be between 6 and 20 characters long")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "User Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "User Name must be between 3 and 50 characters")]
        public string UserName { get; set; }


        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Phone number must start with 010 or 011 , 012 , 015 and be 11 digits long")]
        public string PhoneNumber { get; set; }
    }
}
