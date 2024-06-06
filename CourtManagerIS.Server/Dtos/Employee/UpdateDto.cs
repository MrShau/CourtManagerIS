using CourtManagerApi.Models;

namespace CourtManagerApi.Dtos.Employee
{
    public class UpdateDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public EducationType? EducationType { get; set; }
        public string? Login { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }
        public string? SeriePassport { get; set; }
        public string? NumberPassport { get; set; }
    }
}
