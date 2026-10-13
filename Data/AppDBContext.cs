using Microsoft.EntityFrameworkCore;
using MyRestAPI.Entities;

namespace MyRestAPI.Data
{
    //STEP 3: HERE I NEED TO CREATE DB CONTEXT CLASS, THAT MAPS THE BUSINESS ENTITIES INTO THE TABLE, THIS CONVERT THE CLASS INTO TABLE USING EF CORE PACKAGES.
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        //STEP 4: HERE I NEED TO DEFINE THE DBSET [TABLES THAT CREATED INSIDE THE DATABASE], ONCE DONE, GO TO APPSETTING AND DO CONNECTION STIRNG, THEN REGISTER DBCONTEXT FILE AND CONNECTION STIRNG TO PRORAM.CS FILE.:
        public DbSet<User> AccountUser { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
