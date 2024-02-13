namespace InternManagement.Models
{
    // Giảng viên
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        // trình độ: Thạc sĩ
        public string? Degree { get; set; }

        // bộ môn
        public string? Subject { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
