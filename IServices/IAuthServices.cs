using MyRestAPI.DTO;

namespace MyRestAPI.IServices
{
    public interface IAuthServices
    {
        //STEP 8: HERE WE NEED TO DECLARE ONLY THE METHOD IMPLEMENTATION IN THE INTERFACE CLASS: 
        Task<Tuple<int, TokenDto>> LoginUser(UserDto dto);

        Task<Tuple<int, string>> RegisterUser(UserDto dto);



    }
}
