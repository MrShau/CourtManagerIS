using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CourtManagerApi.Models
{
    public class CourtCase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(5012)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [AllowNull]
        public DateTime? EndDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int JudgeId { get; set; }
        public Employee Judge { get; set; }

        public List<Participant> Participants { get; set; } = new List<Participant>();

        public DateTime CreatedAt { get; set; }

        CourtCase()
        {
            EndDate = null;
            CreatedAt = DateTime.Now;
        }

        public CourtCase(string number, string name, string description, DateTime startDate, string status, Employee judge) : this()
        {
            Number = number;
            Name = name;
            Description = description;
            StartDate = startDate;
            Status = status;
            Judge = judge;
        }
    }
}
