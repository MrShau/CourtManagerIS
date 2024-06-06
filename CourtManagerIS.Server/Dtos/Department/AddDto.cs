
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CourtManagerApi.Dtos.Department
{
    public class AddDto
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(2500)] 
        public string Description { get; set; }

        [AllowNull]
        public int? SupervisorId { get; set; }
    }
}
