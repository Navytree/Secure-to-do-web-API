using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ToDoUserWebAPI.Models;


namespace ToDoUserWebAPI.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }
        public DbSet<ToDo> ToDos { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>()
                .HasOne(e => e.User)  
                .WithMany(e => e.Todos)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }


    }
}
