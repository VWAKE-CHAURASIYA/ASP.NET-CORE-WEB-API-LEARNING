using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyRestAPI.Data;
using MyRestAPI.IServices;
using MyRestAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>(item => item.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();

//STEP 20: ONCE I CREATED ALL THE ENDPOINTS FOR THE SERVICES, WE MUST HAVE TO MAKE IT SECURED, USING AUTHENTICATION, OTHERWISE IT CAN BE CORRUPTED BY SOMEONE.. AND PROVIDE PREVILEGE USING AUTHERIZATION: TO USE IT FIRST WE NEED TO INSTALL A PACKAGE CALLED AS {JWT BEARER} AND THEN REGISTER IT ON THE PROGRAM.CS FILE, AND DO ALOTS OF CONFIGURATION TO USE IT.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //NOW WE NEED TO AUTHENTICATE OR VALIDATE THE JWT BEARERES TOKENS, FOR THAT WE NEED TO DO SETTINGS , SO GO TO <APPSETTING.JSON> AND DEFINE PRODUCTION LEVEL WAY TO DEVELOP THE TOKEN FOR API.
    //ONCE GETTING THE SECRET JWT TOKENS, WE NEED TO PASS TO THE JWTBEARER.
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;  //TO SAVE THE TOKENS 

    //NOW HERE WE NEED TO PERFORM VALIDATION FOR JWT BEARER: 
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer =builder.Configuration.GetValue<string>("JWT:Issuer"), //WE CAN ACCESS THE ISSUES, FROM THE APPSETTING.JSON VALUE, USING CONFIGURATION.
        ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"))),
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//STEP 22: NOW DEFINING THE MIDDLEWARE HERE FOR JWT AUTHENTICATION, JUST BEFORE, THE AUTHERIZATION MIDDLEWARE: [ THEN GO TO 'EMPLOYEES' ACTIONS METHODS /CONTROLLER, AND MAKE IT AUTHORIZE ]
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
