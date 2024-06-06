using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Models
{
    public enum Privilege
    {
        ADMIN,
        DIRECTOR,
        EMPLOYEE,
        JUDGE
    }

    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public Privilege Privilege { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Candidate> Candidates { get; set; } = new List<Candidate>();

        Position() => CreatedAt = DateTime.Now;
        public Position(string name, Department department, Privilege privilege, string description = "") : this()
        {
            Name = name;
            Department = department;
            Description = description;
            Privilege = privilege;
        }
    }
}
