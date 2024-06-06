using System.ComponentModel.DataAnnotations;

namespace CourtManagerIS.Server.Dtos.Event
{
    public class AddEventDto
    {
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
    }
}
