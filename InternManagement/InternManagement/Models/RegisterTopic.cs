namespace InternManagement.Models
{
    // Đăng kí đề tài
    public class RegisterTopic
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TopicId { get; set; }
        public int TeamId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
