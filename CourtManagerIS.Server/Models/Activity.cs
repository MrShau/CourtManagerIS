
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CourtManagerApi.Models
{
    public class Activity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime SignInTime { get; set; }

        [Required]
        [JsonIgnore]
        public int EmployeeId { get; set; }
        [JsonIgnore]
        public Employee Employee { get; set; }

        [Required]
        public DateTime SignOutTime { get; set; }

        [AllowNull]
        public string Description { get; set; }

        Activity()
        {
            SignInTime = DateTime.Now;
            Description = "";
            SignOutTime = DateTime.Now;
        }

        public Activity(Employee employee) : this()
        {
            Employee = employee;
        }

    }
}
