using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyRestAPI.Data;
using MyRestAPI.DTO;
using MyRestAPI.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyRestAPI.Services
{
    //STEP 6: CREATED A FOLDER FOR SERVICES AND ISERVICES, SO THAT WE CAN PERFORM THE ABSTRACITON: 
    //WE DESIGN THE ENDPOINTS ON CONTROLLER PAGE, AND THE INTERNAL THINGS ARE DEFINED INSIDE SERVICES.

    public class AuthServices : IAuthServices
    {
        private readonly AppDBContext context;

        // HERE I CREATE A PARAMETERIZED CONSTRUCTOR, THAT TAKES THE DBCONTEXT REFERECNE USING DI, BY WHICH WE GETS IT OBJECT, TO USE IT AS IN OUR ACTION METHODS. 
        public AuthServices(AppDBContext context)
        {
            this.context = context;
        }

        //NOW HERE I NEED TO CREATE A METHODS FOR LOGIN AND REGISTERATION, THAT THEIR LINK MUST BE PRESENT AT [IAUTHSERVICE].
        //[THE CLASS MUST INHERIT THE INTERFACE, INTERFACE SERVICES ARE PROVIDING ABSTRACTION, THAT HELPS TO HIDES THE INTERNAL INFORMATION, THAT MAKE IT SECURE.

        //public async Task<bool> LoginUser(UserDto dto)   // NOTE: WE MUST HAVE TO REGISTER THE SERVICES AND ISERVICES FILES ON PROGRAM.CS FILE..
        //public async Task<Tuple<int, string>> LoginUser(UserDto dto)  //NOTE HERE NEVER PASS ONLY THE SPECIFIC OR REQUIRED VALUES, WHICH WE WANT TO PASS TO API'S.
        public async Task<Tuple<int, TokenDto>> LoginUser(UserDto dto)
        {
            //IT'S A GOOD PREACTISE , WHEN WORK INSIDE SERVICES WE USE EXCETPIONAL HANDLING: 
            try
            {
                var tokenDto = new TokenDto(); //MAKE IT GLOBALLY FOR ALL..

                if (dto == null)
                {
                    //STEP 27: NOW USE THE TOKEN DTO HERE: 

                    //var tokenDto = new TokenDto
                    //{
                    //tokenDto.Token = string.Empty;    // HERE IF WE HAVE VERIFIED DATA, THEN WE NEED TO PASS THE TOKEN. 
                    tokenDto.Message = "Please Fill All the Details...";
                    //};

                    //return new Tuple<int, string>(2, "Login Successfully");
                    return new Tuple<int, TokenDto>(2, tokenDto); //PASS THE VARIABLE 'tokenDto'.
                    //return new Tuple<int, string>(0, "Please Fill All the Details..");
                }

                //HERE INSIDE LOGIN FUNCTIONALITY, I NEED TO CHECK WEATHER THE USER IS REGISTERED/EXISTING USER OR NOT: 
                var existingData = await context.AccountUser.FirstOrDefaultAsync(x => x.Email == dto.Email);


                //FIND USER BY EMAIL: 
                if (existingData == null)
                {


                    //STEP 27: NOW USE THE TOKEN DTO HERE: 

                    //var tokenDto = new TokenDto
                    //{
                    //tokenDto.Token = string.Empty;    // HERE IF WE HAVE VERIFIED DATA, THEN WE NEED TO PASS THE TOKEN. 
                    tokenDto.Message = "User is Not Exist...Please Login.. ";
                    //};
                    return new Tuple<int, TokenDto>(2, tokenDto); //PASS THE VARIABLE 'tokenDto'.

                    //return new Tuple<int, string>(1, "User is Not Exist...Please Login.. "); ;
                }


                //STEP 13: [VERY VERY IMPORTANT] NOW DEFINING THE CONDITION FOR GETTING HASHED PASSWORD WHILE LOGIN: 
                var passwordHasher = new PasswordHasher<string>();
                var verifyPassword = passwordHasher.VerifyHashedPassword(dto.Email, existingData.Password, dto.Password);

                if (verifyPassword == PasswordVerificationResult.Success)
                {
                    //STEP 25: NOW IMPLEMENTING IT BEFORE SUCCESS,  VALIDATION FOR LOGIN, FOR BOTH CONDITIONS: 
                    UserDto user = new();
                    user.Email = existingData.Email;
                    user.Id = existingData.Id;
                    user.Name = existingData.Name;
                    var token = GetJWTTokens(user);


                    //STEP 27: NOW USE THE TOKEN DTO HERE: 

                    //var tokenDto = new TokenDto
                    //{
                    tokenDto.Token = token;    // HERE IF WE HAVE VERIFIED DATA, THEN WE NEED TO PASS THE TOKEN. 
                    tokenDto.Message = "login Successfully...";
                    //};
                    return new Tuple<int, TokenDto>(2, tokenDto); //PASS THE VARIABLE 'tokenDto'.

                    //return new Tuple<int, string>(2, "Login Successfully");
                }

                //IF PASSWORD IS VERIFIED, BUT NEEDS TO GET A NEW PASSWORD,SO I UPDATE THE NEW PASSWORD IN EXISTING USER. 
                else if (verifyPassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    //STEP 25: NOW IMPLEMENTING IT BEFORE SUCCESS,  VALIDATION FOR LOGIN, FOR BOTH CONDITIONS: 
                    UserDto user = new();
                    user.Email = existingData.Email;
                    user.Id = existingData.Id;
                    user.Name = existingData.Name;
                    var token = GetJWTTokens(user);

                    existingData.Password = PasswordHashing(dto);
                    context.AccountUser.Update(existingData);
                    context.SaveChanges();

                    //STEP 27: NOW USE THE TOKEN DTO HERE: 
                    //var tokenDto = new TokenDto
                    //{
                    tokenDto.Token = token;   //AS IT IS VALID.
                    tokenDto.Message = "Login Successfully, New Hash Generated... ";
                    //};


                    return new Tuple<int, TokenDto>(2, tokenDto);

                    //return new Tuple<int, TokenDto>(2, "Login Successfully, Hash Password Generated...");  //NOW MAKE THE TUPLE AS TOKENDTO TYPE>
                }


                else if(verifyPassword == PasswordVerificationResult.Failed)
                {

                    //STEP 27: NOW USE THE TOKEN DTO HERE: 
                    //var tokenDto = new TokenDto
                    //{
                    //tokenDto.Token = string.Empty; 
                    tokenDto.Message = "Password Incorrect... ";
                    //};
    
                    return new Tuple<int, TokenDto>(0, tokenDto);

                    //return new Tuple<int, string>(1, "Password Incorrect...");
                }

                else
                {
                    tokenDto.Message = "Something Went Wrong...";

                    return new Tuple<int, TokenDto>(3, tokenDto);
                }

            }

            catch (Exception)
            {
                var tokenDto = new TokenDto();
                tokenDto.Message = "Something Went Wrong...";
                return new Tuple<int, TokenDto> ( 3, tokenDto );
                //return new Tuple<int, string>(3, "Something Went Wrong...");
            }
        }


        // STEP 24: NOW CREATING A METHOD FOR  GENERATING THE JWT TOKEN: 
        private string GetJWTTokens(UserDto dto)
        {
            var claims = new[]      //A Claim is a piece of information about a user, represented by a key-value pair. Claims are used to store information about an authenticated user.
            {
                new Claim(ClaimTypes.Name, dto.Name),
                new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("94769a376a15bf2d3868dca9a2675aba23a68b9592e67fc83d4f288c0e41057c"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);  // Credentials are the information used to establish trust or prove identity

            var token = new JwtSecurityToken(     //Token is a piece of data that a client sends to the server to prove its authenticated identity when accessing protected resources.
                issuer: "Vivek- Client",
                audience: "Vivek - Backend",
                claims: claims,

                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);  //JWT is normally generated after successful authentication/login, not before login succeeds.
        }


        //STEP 10: NOW CREATE A SERVICE FOR 'REGISTRATION', THAT WE USE IT INSIDE AUTHCONTROLLER.
        //[REMEMBER: WHENEVER WE CREATE ANY TYPE OF SERVICES, WE GET AN EXTRA LAYER CALLED AS "REPOSITORY LAYER" ON CLEARN ARCHTECTURE..,BUT IT IS NOT PRESENT HERE]
        public async Task<Tuple<int, string>> RegisterUser(UserDto dto)  //MUST BE REGISTERD TO AN ISERVICE FOLDER, TO ABSTRACT THE INTERNAL INFORMATION. 
        {
            //HERE AGAIN WE NEED TO CHECK THE CONDITONS FIRST , FOR USER REGISTRAION: 
            try
            {
                if (dto == null) { return new Tuple<int, string>(1, "Please Fill All Details.."); }

                var existingUser = await context.AccountUser.AnyAsync(x => x.Email == dto.Email);  //FOR OPTIMIZED RESULT, WE CAN USE 'ANYASYNC' THAT RETURN BOOLEAN VALUES.IT'S NOT NECESSARY TO USE ONLY THIS METHOD, EVERY METHODS HAVE SPECIFIC FUNCTIONALITY. 

                if (existingUser)
                {
                    return new Tuple<int, string>(0, "This User is Already Existed...Please Register with new user");
                }
                //IF THERE IS A NEW USER, THAN WE NEED TO REGISTER IT: 
                context.AccountUser.Add(new Entities.User    // HERE WE ADD THE NEW USER , INTO THE SAME TYPE OF MODEL
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    //Password = dto.Password,
                    Password = PasswordHashing(dto),
                });
                await context.SaveChangesAsync();
                return new Tuple<int, string>(1, "");

            }
            catch (Exception)
            {
                throw;
            }
        }

        //STEP 12: HERE I NEED TO PEFORM THE SERVICES FOR 'PASSWORD HASHING' AS A PRIVATE TYPE, SO THAT NO ONE CAN ABLE TO ACCESS IT, AND THEN DEFINE IT TO REGISTER AND LOGIN METHOD FOR PASSWORD HASING.. : 
        private string PasswordHashing(UserDto dto)  // HERE WE PASS THE DTO MEMEBERS TO BE CHECKED.
        {
            var passwordHasher = new PasswordHasher<string>();  //HERE WE DO PASSING HASHING, BY CREATING AN OBJECT FOR 'PASSWORDHASHER' CLASS [ THESE CAN BE AUTOMATICALLY CREATED USING [ASP.NET CORE IDENTITY FRAMEWORK ]. 
            var hash = passwordHasher.HashPassword(dto.Email, dto.Password);  //HERE WE NEED TO PASS THE USER ALONG WITH PASSWORD TO CHECK ITS ASSOCIATED WITH IT.
            return hash;
        }
    }
}
