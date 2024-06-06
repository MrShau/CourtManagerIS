using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Dtos.Notification
{
    public class SendDto
    {
        [Required]
        public int RecipientId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(2500)]
        public string Content { get; set; }
    }
}
