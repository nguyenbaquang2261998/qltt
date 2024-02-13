namespace InternManagement.DTOs.Student
{
    public class CreateStudentBindingModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? SubPhone { get; set; }
        public string? Identity { get; set; }
        public string? Address { get; set; }
        public int ClassCode { get; set; }
        public DateTime Birthday { get; set; }
    }
}
