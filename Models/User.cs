namespace TeeMate_ServerSide.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? Handicap { get; set; }
        public string? Availability { get; set; }
        public string? Transportation { get; set; }
        public string? Clubs { get; set; }
        public string? ProfilePic { get; set; }
        public string? Uid { get; set; }
        public int SkillLevelId { get; set; }
        public SkillLevel? SkillLevel { get; set; }
        public List<TeeTime>? TeeTimes { get; set; }
    }
}
