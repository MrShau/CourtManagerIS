using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Dtos.Department
{
    public class UpdateDepartmentDto
    {
        public int Id { get; set; }
        [StringLength(255, MinimumLength = 2)]
        public string? Name { get; set; }
        [StringLength(2500, MinimumLength = 2)]
        public string? Description { get; set; }
    }
}
