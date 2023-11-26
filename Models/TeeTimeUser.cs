namespace TeeMate_ServerSide.Models
{
    public class TeeTimeUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeeTimeId { get; set; }
        public TeeTime? TeeTime { get; set; }
        public User? User { get; set; }
    }
}
