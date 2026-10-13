using Microsoft.EntityFrameworkCore;
using MyRestAPI.Data;
using MyRestAPI.DTO;
using MyRestAPI.Entities;
using MyRestAPI.IServices;

namespace MyRestAPI.Services
{
    //STEP 14: NOW PERFORMING SIMILAR OPEATION LIKE, TO CREATE SERVICES, ISERVICES, AND CONTROLLER , REGISTER BOTH SERVICES ON PROGRAM.CS FILE TO USE, MAKE DTO FILE, ENTITY FILES ETC. JUST LIKE AS FOR PERFORMING AUTHENTICATION. 
    public class EmployeeServices(AppDBContext context) : IEmployeeServices  //HERE WE CAN INSERT OUR DBCONTEXT USING PRIMARY CONSTRUCTORY WITH THE CLASS, [ASP.NET CORE VERSION 8 FEATURES]
    {
        //NOW CREATING A METHOD FOR GETTING THE EMPLOYEE DATA, THESE DATA MUST BE ACCEESSED FORM DTO FILE, INSTEAD OF MAIN ENTITY.: 
        public async Task<Tuple<int, List<EmpDto>>> GetAllEmployee()
        {
            try
            {
                return new Tuple<int, List<EmpDto>>(1, await context.Employees.AsNoTracking().Select(x => new EmpDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedDate = x.CreatedDate,
                    LastDate = x.LastDate,
                    DOB = x.DOB,
                    Position = x.Position,
                    Email = x.Email,
                }).ToListAsync()); //HERE WE ARE PASS TWO ITEMS IN TUPLE,INT AND LIST TYPE, BUT WE CAN'T PASS LIST DIRECTLY WITH TUPLE, SO HERE WE USE 'PROJECTION' THAT CONVERTS ONE MODEL CLASS TYPE TO ANOTHER MODEL CLASS TYPE [USING PROJECTION METHODS LIKE SELECT()]. //AND IF WE WANT TO OPTIMIZED OUR API ACCESS TIME, WE CAN USE A METHOD CALLED "ASNOTRACKING()"METHOD, ITS BENEFIT, IS IT DOESN'T TRACK ANY ENTITY, IF WE WANT TO JUST READ THEN ONLY WE USE THIS METHOD, AND ITS MAKES FAST THE QUERY.       
            }
            catch (Exception)
            {
                throw;
            }
        }

        //STEP 16: METHOD FOR CREATING 'CREATE' METHOD: 
        public async Task<Tuple<int, string>> CreateEmployees(EmpDto dto)
        {
            try
            {
                //FIRST CHECK WEAHTER THERE IS ANY DATA OR NOT USING ANYASYNC: 
                var existing = await context.Employees.AnyAsync(x => x.Email == dto.Email);

                if (existing)
                {
                    return new Tuple<int, string>(1, "Employee Already Exist..");
                }
                //IF NOT, ADD THE NEW ENTRIES:
                await context.Employees.AddAsync(new Entities.Employee
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    CreatedDate = DateTime.Now,
                    LastDate = null,
                    DOB = dto.DOB,
                    Position = dto.Position,
                    Email = dto.Email,
                });
                await context.SaveChangesAsync();
                return new Tuple<int, string>(2, "Employee Created Successfully..");
            }
            catch (Exception)
            {
                throw;
            }
        }


        //STEP 17 : METHOD FOR CREATING 'UPDATE' METHOD: 
        public async Task<Tuple <int, string>> UpdateEmployees(EmpDto dto)
        {
            try
            {
                //FIRST CHECK WEAHTER THERES IS ANY DATA OR NOT: 
                var existing = await context.Employees.FirstOrDefaultAsync(x => x.Email == dto.Email);

                if (existing == null)
                {
                    return new Tuple<int, string>(0, "Employee Not Exist With This Email Id...");
                }

                //IF THE DATA IS THERE, THAN WE NEED TO UPDATE IT:
                //existing.CreatedDate = DateTime.Now;
                //existing.LastDate = null;
                existing.Name = string.IsNullOrWhiteSpace(dto.Name) ? existing.Name :dto.Name;   //MEANS IF THE DTO.NAME IS EMPTY, THAN KEEP OLD NAME, MEANS NOT ANY UPDATION FOR NAME.
                existing.DOB = dto.DOB ?? existing.DOB; //null-coalescing operator ??: If dto.DOB has a value, use it. If dto.DOB is null, keep the existing DOB.
                existing.Position = string.IsNullOrWhiteSpace(dto.Position) ?  existing.Position : dto.Position;
                existing.Email = string.IsNullOrWhiteSpace(dto.Email) ? existing.Email : dto.Email;

                await context.SaveChangesAsync();
                return new Tuple<int, string>(1, "Employee Updated Successfully..");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //STEP 18: METHOD FOR DELETION: 
        public async Task<Tuple<int, string>> DeleteEmployee(Guid id)
        {
            try
            {
                //GET THE ID WHICH WE WANT TO DELETE: 
                var data = await context.Employees.FirstOrDefaultAsync(x => x.Id == id);

                if (data == null)
                {
                    return new Tuple<int, string>(0, "Item is not found");
                }
                context.Employees.Remove(data);
                await context.SaveChangesAsync();

                return new Tuple<int, string>(1, "Item has Deleted Successfully...");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //STEP 19: METHOD/SERVICES FOR GETTING A SPECIFIC DATA. 
        public async Task<Tuple<int, EmpDto?>> GetElementById(Guid id)  //HERE WE PASS A TUPLE WITH THE INT VALUE, WITH THE 'DTO' TO ACCESS THE PARTICULAR QUERY, AND HERE WE USE GUID ID, AS WE STORE ID OF GUID TYPES.
        {
            var data = await context.Employees.AsNoTracking().Select(x => new EmpDto
            {
                Id = x.Id,
                Name = x.Name, 
                DOB = x.DOB,
                LastDate = x.LastDate,
                Email = x.Email,
                Position = x.Position,
                CreatedDate = x.CreatedDate,

            }).FirstOrDefaultAsync(x => x.Id == id);  //HERE ALSO WE PASS 'GUID ID' TO COMPARE..

            if(data == null)
            {
                return new Tuple<int, EmpDto? >(0, null); //Here,C# gives a nullable warning,Because EmpDto is probably a non-nullable reference type, So i need to make the tuple as nullable..
            }
            return new Tuple<int, EmpDto?>(1, data);
        }

    }
}
