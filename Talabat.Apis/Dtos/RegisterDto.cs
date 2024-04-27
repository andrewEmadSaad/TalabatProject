using System.ComponentModel.DataAnnotations;

namespace Talabat.Apis.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string  DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
        [Phone]

        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
            ErrorMessage = " At least one upper case english letter , At least one lower case english letter , At least one digit , At least one special character , Minimum 8 in length")]
        public string  Password { get; set; }
    }
}
