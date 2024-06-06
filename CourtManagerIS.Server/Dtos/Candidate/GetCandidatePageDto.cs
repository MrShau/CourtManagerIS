namespace Api.Dtos.Candidate
{
    public class GetCandidatePageDto
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public byte WorkExperience { get; set; }
        public int DesiredPositionId { get; set; }
        public string DesiredPosition { get; set;}
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
