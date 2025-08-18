using System.ComponentModel.DataAnnotations;

namespace StaffManagement.API.Models
{
    public class Staff
    {
        [Key]
        [StringLength(8)]
        [Display(Name = "Full Id")]
        public string? StaffId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required]
        public DateOnly Birthday { get; set; }

        [Required]
        public int Gender { get; set; }
    }
}
