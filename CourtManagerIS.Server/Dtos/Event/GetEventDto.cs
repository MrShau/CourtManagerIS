namespace CourtManagerIS.Server.Dtos.Event
{
    public class GetEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreaterFIO { get; set; }
    }
}
