namespace InternManagement.Models
{
    // Đề tài
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        // tham khảo
        public string? Reference { get; set; }
        // kì hạn
        public string? Deadline { get; set; }
        // GV hướng dẫn
        public int? TeacherId { get; set; }
        // Kì thực tập
        public int? SemesterId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
