namespace InternManagement.Models
{
    // Giảng viên đánh giá thực tập
    public class TeacherEvaluation
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int TopicId { get; set; }
        public int TopicName { get; set; }
        // điểm thái độ
        public int AttitudePoint { get; set; }
        public string AttitudeReview { get; set; }
        // điểm tiến độ
        public int ProgressPoint { get; set; }
        public string ProgressReview { get; set; }
        // điểm chất lượng
        public int QualityPoint { get; set; }
        public string QualityReview { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
