using checkers_api.Models;
using Microsoft.EntityFrameworkCore;

namespace checkers_api.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {

    }
}