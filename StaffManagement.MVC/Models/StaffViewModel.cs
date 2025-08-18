namespace StaffManagement.MVC.Models
{
    public class StaffViewModel
    {
        public string StaffId { get; set; }
        public string? FullName { get; set; }
        public DateOnly Birthday { get; set; }
        public int Gender { get; set; }
    }
}
