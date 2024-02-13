namespace InternManagement.Models
{
    // Sinh viên đánh giá kì thực tập
    public class InternshipEvaluation
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        // đánh giá giảng viên
        public string EvaluateTeacher { get; set; }
        // đánh giá đề tài
        public string EvaluateTopic { get; set; }
        public string? Docs { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
