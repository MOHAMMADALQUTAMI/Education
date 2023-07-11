using Microsoft.EntityFrameworkCore;

namespace ManyToMany.Model
{
    public class AppDbContext:DbContext
    {
        public DbSet<Student> Students {get;set;}
        public DbSet<Address> Addresses {get;set;}
        public DbSet<Course> Courses {get;set;}
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }
    }
}