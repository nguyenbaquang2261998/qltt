namespace InternManagement.DTOs.Evaluation
{
    public class RegisterTopicStudent
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public int TeamId { get; set; }
        public string Deadline { get; set; }
        public int Status { get; set; }
    }
}
