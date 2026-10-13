using System.ComponentModel.DataAnnotations;

namespace MyRestAPI.DTO
{
    public class EmpDto
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? LastDate { get; set; }

        public DateTime? DOB { get; set; }
        public string? Position { get; set; }

        public string? Email { get; set; }
    }
}

