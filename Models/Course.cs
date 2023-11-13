namespace TeeMate_ServerSide.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Par { get; set; }
        public string? HoleCount { get; set; }
        public decimal Cost { get; set; }
        public string? Image { get; set; }
    }
}
