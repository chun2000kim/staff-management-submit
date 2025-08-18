namespace StaffManagement.API.DTOs
{
    public class StaffSearchDto
    {
        public string? StaffId { get; set; }
        public int? Gender { get; set; }
        public string? BirthdayFrom { get; set; }
        public string? BirthdayTo { get; set; }

    }
}
