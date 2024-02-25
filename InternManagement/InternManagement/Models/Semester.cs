namespace InternManagement.Models
{
    // Kì thực tập
    public class Semester
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Mô tả

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Description { get; set; }
        //Số lượng sv tối đa
        public int Target { get; set; }
        // Số sv đã đăng kí
        public int TotalRegis { get; set; }
    }
}
