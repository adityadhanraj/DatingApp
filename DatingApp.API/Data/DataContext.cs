using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;
namespace DatingApp.API.Data
{
    //DataContext class
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Values> Values { get; set; }
        public DbSet<User> Users { get; set; }
    }
}