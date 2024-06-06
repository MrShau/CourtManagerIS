using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }
        public Employee Sender { get; set; }

        [Required]
        [StringLength(512)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        ChatMessage()
        {
            CreatedAt = DateTime.Now;
        }

        public ChatMessage(Employee sender, string content) : this()
        {   
            Sender = sender;
            Content = content;
        }
    }
}
