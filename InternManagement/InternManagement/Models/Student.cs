namespace InternManagement.Models
{
    // Sinh viên
    public class Student
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? SubPhone { get; set; }
        public string? Identity { get; set; }
        public string? Address { get; set; }
        public string? ClassName { get; set; }
        public int ClassCode { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
