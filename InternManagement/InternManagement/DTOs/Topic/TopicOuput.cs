namespace InternManagement.DTOs.Topic
{
    public class TopicOuput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string? Deadline { get; set; }
        public string? Reference { get; set; }
        public string Teacher { get; set; }
        public string Semester { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
