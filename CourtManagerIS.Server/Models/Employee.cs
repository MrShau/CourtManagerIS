using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Models
{
    [Index(nameof(Login), IsUnique = true)]
    [Index(nameof(PersonId), IsUnique = true)]
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 7)]
        public string PasswordHash { get; set; }

        [Required]
        public byte PasswordAttempts { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfAppointment { get; set; }

        [Required]
        public bool Status { get; set; }

        public List<Activity> ActivityHistory { get; set; } = new List<Activity> { };

        public List<Notification> SentNotifications { get; set; } = new List<Notification> { };
        public List<Notification> Notifications { get; set; } = new List<Notification> { };

        public List<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage> { };

        Employee()
        {
            IsBlocked = false;
            Status = true;
            DateOfAppointment = DateTime.Now;
            PasswordAttempts = 0;
            ImagePath = "/images/profile.jpg";
        }

        public Employee(string login, string password, Person person, Position position) : this()
        {
            Login = login.ToLower();
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            Person = person;
            Position = position;
        }

        public bool VerifyPassword(string password) => BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        public void ChangePassowrd(string password)
        {
            if (password.Length >= 7 && password.Length <= 255)
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
