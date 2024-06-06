using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Models
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FIO { get; set; }

        [Required]
        [StringLength(255)]
        public string Role { get; set; }

        [Required]
        [StringLength(512)]
        public string ContactInfo { get; set; }

        [Required]
        public int CourtCaseId { get; set; }
        public CourtCase CourtCase { get; set; }

        Participant() { }
        public Participant(string fIO, string role, string contactInfo, CourtCase courtCase)
        {
            FIO = fIO;
            Role = role;
            ContactInfo = contactInfo;
            CourtCase = courtCase;
        }
    }
}
