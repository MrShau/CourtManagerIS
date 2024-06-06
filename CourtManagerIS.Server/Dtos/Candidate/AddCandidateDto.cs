using CourtManagerApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Candidate
{
    public class AddCandidateDto
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
        public EducationType EducationType { get; set; }

        [Required]
        public int DesiredPositionId { get; set; }

        [Required]
        public byte WorkExperience { get; set; }
    }
}
