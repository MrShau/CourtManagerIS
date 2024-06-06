using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte WorkExperience { get; set; }

        [Required]
        public int DesiredPositionId { get; set; }
        public Position DesiredPosition { get; set; }

        [Required]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        Candidate() => CreatedAt = DateTime.Now;
        public Candidate(Position position, Person person, byte workExperience) : this()
        {
            WorkExperience = workExperience;
            DesiredPosition = position;
            Person = person;
        }
    }
}
