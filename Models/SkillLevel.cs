namespace TeeMate_ServerSide.Models
{
    public class SkillLevel
    {
        public int Id { get; set; }
        public string? Level { get; set; }
        public List<User>? Users { get; set; }
    }
}
