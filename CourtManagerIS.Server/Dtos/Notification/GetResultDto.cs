namespace CourtManagerApi.Dtos.Notification
{
    public class GetResultDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public string SenderFIO { get; set; }
        public int RecipientId { get; set; }
        public string RecipientFIO { get; set; }
    }
}
