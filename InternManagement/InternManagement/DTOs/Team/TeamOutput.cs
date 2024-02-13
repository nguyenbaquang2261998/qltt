namespace InternManagement.DTOs.Team
{
    public class TeamOutput
    {
        public int Id { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public int TeamSize { get; set; }
        public List<StudentRegister>? Students { get; set; }
        public StudentInfo StudentInfo { get; set; }
    }

    public class StudentRegister
    {
        public int Id { get; set; }
        public string Identity { get; set; }
        public string Name { get; set; }
    }
    
    public class StudentInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identity { get; set; }
    }
}
