namespace CourtManagerApi.Dtos.Department
{
    public class GetListResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EmployeesCount { get; set; }
        public int PositionsCount { get; set; }
        public string Positions { get; set; }
        public int SupervisorId { get; set; }
        public string Supervisor { get; set; }
    }
}
