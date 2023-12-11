namespace TeeMate_ServerSide.Models
{
    public class TeeTime
    {
        public int Id { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Location { get; set; }
        public int NumOfPlayers { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public List<User>? Users { get; set; }
    }
}
