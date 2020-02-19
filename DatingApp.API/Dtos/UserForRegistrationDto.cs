using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegistrationDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(20,MinimumLength=5,ErrorMessage="Password lenght should be between 5 to 20 characters")]
        public string Password { get; set; }
    }
}