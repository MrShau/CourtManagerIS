using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CourtManagerApi.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }
        [JsonIgnore]
        public Employee Sender { get; set; }

        [Required]
        public int RecipientId { get; set; }
        [JsonIgnore]
        public Employee Recipient { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(2500)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        Notification()
        {
            CreatedAt = DateTime.Now;
        }

        public Notification(Employee sender, Employee recipient, string title, string content) : this()
        {
            Sender = sender;
            Recipient = recipient;
            Title = title;
            Content = content;
        }
    }
}
