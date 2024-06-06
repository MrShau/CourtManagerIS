namespace Api.Dtos.Employee
{
    public class GetActivityHistoryResultDto
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string Duration { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
    }
}
