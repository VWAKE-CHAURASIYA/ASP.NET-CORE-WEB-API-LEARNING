using System.ComponentModel.DataAnnotations;

namespace MyRestAPI.Entities
{
    //STEP 1: HERE I NEED TO CREATE A FOLDER, ENTITES(USED FOR DATABASE/DOMAIN ENTITIES), HERE WE USED TO PERFORM LOGIN AND REGISTRATION WITH PASSWORD HASHING...
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); //IT RETURN UNIQUELY IDENTICAL KEYS OF 32 HEXADECIMAL CHARACTERS.
        public string? Name { get; set; }

        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
