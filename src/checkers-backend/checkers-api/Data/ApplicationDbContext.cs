using checkers_api.Models;
using Microsoft.EntityFrameworkCore;

namespace checkers_api.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {

    }
    
    public DbSet<User> Users { get; set; }
}