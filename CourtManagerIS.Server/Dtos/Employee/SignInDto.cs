using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Dtos.Employee
{
    public class SignInDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 7)]
        public string Password { get; set; }

    }
}
