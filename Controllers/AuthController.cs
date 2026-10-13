
using Microsoft.AspNetCore.Mvc;
using MyRestAPI.DTO;
using MyRestAPI.Generics;
using MyRestAPI.IServices;

//STEP 5: ONCE MIGRATION DONE, HERE WE CREATE A CONTROLLER CLASS FOR PEFORMING AUTHENTICATION, AND THEN CREATE A SERVICES WIICH WE WANT, AND DEFINE THE ENDPOINTS HERE IN THE CONTROLLERS: 
namespace MyRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices auth;  //REMEMBER: [Your controller should use the interface Class]

        //STEP 7: NOW FOR CREATING ROUTES, FIRST I NEED TO CREATE AN OBJECT OF IAUTHSERVICE USING DI, FOR THAT WE CREATE IT INSIDE P. CONSTRUCTOR, AND THEN FURTHER WE CREATE ACTION METHODS/END POINTS, TO DEFINE IT.
        // NOW HERE I NEED TO CONSUME THE 'AUTHSERVICE' METHOD, SO FOR THAT ITS BETTER TO USE DI, INSTEAD OF ACCESSING THE OBJECT/METHOD USING DOT OPERATOR OF CLASS. 
        public AuthController(IAuthServices auth)
        {
            this.auth = auth;
        }


        //NOTE: REMEMBER, HERE WE PASS ONLY DTO ENTITIES INSTEAD OF REAL BUSINESS ENTITES. IT TAKES ONLY SPECIFIC DATAS. 
        //NOW HERE WE CREATE 'AN API FOR LOGIN' OR ENDPOINT FOR LOGIN: 
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDto userinfo)
        {
            try
            {
                var result = await auth.LoginUser(userinfo);
                if (result.Item1 == 0)
                {
                    //return NotFound(result.Item2);  
                    return NotFound(ResponseResult<TokenDto>.Failure(result.Item2 , result.Item2.Message));
                    //return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }
                if (result.Item1 == 1)
                {
                    //return BadRequest(result.Item2);
                    return BadRequest(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));
                    //return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }
                //return Ok(result.Item2);
                return Ok(ResponseResult<TokenDto>.Success(result.Item2, result.Item2.Message));
                //return Ok(ResponseResult<string>.Success(null, result.Item2));

            }
            catch (Exception)
            {
                throw;
            }
        }


        //STEP 11: NOW CALLING THE REGISTERATION SERVICES , INSIDE THE CONTROLLER, : 
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserDto userinfo)
        {
            try
            {
               var result = await auth.RegisterUser(userinfo);
                if(result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Success(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
               
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
