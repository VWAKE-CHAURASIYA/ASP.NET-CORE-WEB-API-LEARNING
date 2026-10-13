using System.ComponentModel.DataAnnotations;

namespace MyRestAPI.Entities
{
    //STEP 2: HERE WE CREATE ANOTHER CLASS FOR EMPLOYEE, [HERE WE PEFROM- CRUD BASED API...LATER]
    public class Employee
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();   //PRIMARY KEY
        public string? Name { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? LastDate { get; set; }

        public DateTime? DOB { get; set; }
        public string? Position { get; set; }

        public string? Email { get; set; }
    }
}
