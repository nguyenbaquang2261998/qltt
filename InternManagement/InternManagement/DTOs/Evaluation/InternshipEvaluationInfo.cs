
namespace InternManagement.DTOs.Evaluation
{
    public class InternshipEvaluationInfo
    {
        public StudentInfo? StudentInfo { get; set; }
        public TopicInfo? TopicInfo { get; set; }
        public List<StudentInfo>? StudentInTeams { get; set; }
    }

    public class StudentInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identity { get; set; }
    }

    public class TopicInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int TeacherId { get; set; }
        public string Deadline { get; set; }
    }
}
