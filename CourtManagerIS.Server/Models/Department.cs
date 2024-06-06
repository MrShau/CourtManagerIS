using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CourtManagerApi.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [AllowNull]
        public int? SupervisorId { get; set; }
        [AllowNull]
        public Employee? Supervisor { get; set; }

        [StringLength(2500)]
        public string Description { get; set; }

        public List<Position> Positions = new List<Position>();

        [Required]
        public DateTime CreatedAt { get; set; }

        Department()
        {
            CreatedAt = DateTime.Now;
            SupervisorId = null;
            Supervisor = null;
        }
        
        public Department(string name, string description = "") : this()
        {
            Name = name;
            Description = description;
        }
    }
}
