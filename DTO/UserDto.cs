using System.ComponentModel.DataAnnotations;


//STEP 8: CREATE A DTO FOLDER, FOR CONTAINING ONLY DTO ENTITIES, THAT WE USED WITH API'S.
namespace MyRestAPI.DTO
{
    public class UserDto
    {
        [Key]
        public Guid Id { get; set; } 
        public string Name { get; set; } = string.Empty;
         
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
