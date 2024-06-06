namespace CourtManagerApi.Dtos.Employee
{
    public class GetListResultDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Position { get; set; }
        public string EducationType { get; set; }
        public string Login { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime DateOfAppointment { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string SeriePassport { get; set; }
        public string NumberPassport { get; set; }
        public string Department { get; set; }
    }
}
