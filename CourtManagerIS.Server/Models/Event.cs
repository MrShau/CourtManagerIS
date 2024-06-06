using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(2500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int CreatedEmployeeId { get; set; }
        public Employee CreatedEmployee { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        Event()
        {
            CreatedAt = DateTime.Now;
        }

        public Event(string title, string description, DateTime startDate, DateTime endDate, Employee employee) : this()
        {
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            CreatedEmployee = employee;
        }
    }
}
