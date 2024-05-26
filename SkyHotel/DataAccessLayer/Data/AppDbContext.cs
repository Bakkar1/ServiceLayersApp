using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Model;

namespace DataAccessLayer.Data;

public class AppDbContext : IdentityDbContext<DotNetUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
    base(options)
    {}


    public DbSet<TodoItem> TodoItems { get; set; }
}
