using checkers_api.PersistentModels;
using Microsoft.EntityFrameworkCore;

namespace checkers_api.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {

    }
    
    public DbSet<UserProfile> UserProfiles { get; set; }
}