using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Dtos.Employee
{
    public class ChangePasswordDto
    {
        public int Id { get; set; }
        [MinLength(7)]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
