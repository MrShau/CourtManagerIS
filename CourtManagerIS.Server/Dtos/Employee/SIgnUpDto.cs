using CourtManagerApi.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CourtManagerApi.Dtos.Employee
{
    public class SIgnUpDto
    {
        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Patronymic { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string SeriePassport { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string NumberPassport { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 7)]
        public string Password { get; set; }

        [Required]
        public EducationType EducationType { get; set; }

        [AllowNull]
        [DefaultValue(-1)]
        public int PositionId { get; set; }
    }
}
