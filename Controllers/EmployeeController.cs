using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MyRestAPI.DTO;
using MyRestAPI.Generics;
using MyRestAPI.IServices;
using MyRestAPI.Services;

namespace MyRestAPI.Controllers
{

    //STEP 23: NOW DEFINING THE AUTERIZATION FOR THE ALL THE ENDPOINTS OR ACITONS METHODS, THAT RETURN 401 ERROR, UNTIL THE VALID IS LOGIN IS NOT VERIFIED.
    [Authorize]

    //STEP 15: HERE WE NEED TO PASS THE 'EMPLOYEESERVICES' AS A DI, DIRECLTY INTO THE CLASS, WHICH WE CAN DO IT USING PRIMARY CONSTRUCTOR METHODS.
    [ApiController]
    [Route("api/[controller]")]


    public class EmployeeController(IEmployeeServices EmpData) : ControllerBase  //REMEMBER: HERE WE MUST PASS , INTERFACECLASS AS DI, TO USE IT , SO THAT THE INTERNAL CODE IS SECURED.
    {

        //NOW CREATING THE API ENDPOINTS TO READ THE EMPLOYEE DATA: 
        [HttpGet]
        public async Task<IActionResult> GetEmployee()
        {
            try
            {
                var result = await EmpData.GetAllEmployee(); //HERE WE PASS THE METHODS TO CHECK.

                if (!result.Item2.Any())
                {
                    return Ok(ResponseResult<List<EmpDto>>.Failure(null, "No Employee Found"));
                }
                return Ok(ResponseResult<List<EmpDto>>.Success(null, "Employee Found..."));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost("CreateEmployee")]
        //CREATE THE EMPLOYEE ENDPOINT: 
        public async Task<IActionResult> CreateEmployee([FromBody] EmpDto dto)  //HERE I MUST HAVE TO PASS DTO CLASS OBJECT, ALONG WITH MODEL BINDER.[THESE ARE OF 4-5 TYPES LIEK :FROM BODY, FORM QUERY, FROM HEADER, FORM ROUTES ETC, WHICH ARE USED OT MAP TO A PARAMETER.] 
        {
            try
            {
                var result = await EmpData.CreateEmployees(dto);
                if (result.Item1 == 1)
                {
                    return Ok(ResponseResult<string>.Failure(null, " Employee Already Exist.."));
                }
                return Ok(ResponseResult<string>.Success(null, "Employee Created Sucessfully"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        //UPDATE THE EMPLOYEES ENDPOINT: 

        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmpDto dto)
        {
            try
            {
                var result = await EmpData.UpdateEmployees(dto);
                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }

        //DELETE THE EMPLOYEES ENDPOINT:

        [HttpDelete("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                var result = await EmpData.DeleteEmployee(id);
                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }

        }

        //CREATING AN ENDPOINT TO GET THE SPECIFIC USER INFORMATION: 
        [HttpGet("GetEmployeeById/{id:guid}")]   //HERE WE CAN DEFINE OUR ID, WITH ITS TYPE ALSO.[ id = route parameter name, guid = route constraint/type]
        public async Task<IActionResult> GetEmployeeById([FromRoute] Guid id)
        {
            try
            {
                var result = await EmpData.GetElementById(id);
                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<EmpDto?>.Failure(null, "Employee Not Found"));    //HERE WE NEED TO GET THE QUERY , FOR SPECIFIC ID, SO HERE OUR RETURN TYPE SHOULD BE "DTO" OBJECT, OF NULLABLE TYPE, THAT ACCEPT, IT MAY BE 0. 
                }
                return Ok(ResponseResult<EmpDto?>.Success(null, "Employee Found"));

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
