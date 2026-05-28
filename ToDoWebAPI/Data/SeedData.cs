using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ToDoUserWebAPI.Models;


namespace ToDoUserWebAPI.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ToDoContext(
                serviceProvider.GetRequiredService<DbContextOptions<ToDoContext>>()))
            {
                if (!context.Users.Any())
                {
                    var hasher = serviceProvider.GetRequiredService<IPasswordHasher>();
                    context.Users.AddRange(
                    new User
                    {
                        Login = "Bilbo",
                        PasswordHash = hasher.Hash("qwert"),
                    },
                    new User
                    {
                        Login = "Qwert",
                        PasswordHash = hasher.Hash("qwert"),
                    });
                    await context.SaveChangesAsync();
                }
                var testUser1 = await context.Users.FirstAsync(u => u.Login == "Bilbo");
                var testUser2 = await context.Users.FirstAsync(u => u.Login == "Qwert");

                if (!context.ToDos.Any())
                {
                    context.ToDos.AddRange(
                        new ToDo { UserId = testUser1.Id, Name = "Walk", Done = false, DateTo = DateOnly.Parse("2026-5-25") },
                        new ToDo { UserId = testUser1.Id, Name =  "Cook", Done = true, DateTo = DateOnly.Parse("2026-5-26") },
                        new ToDo { UserId = testUser1.Id, Name = "Clean", Done = false, DateTo = DateOnly.Parse("2026-5-20") },
                        new ToDo { UserId = testUser2.Id, Name = "Bake bread", Done = false, DateTo = DateOnly.Parse("2026-5-31") },
                        new ToDo { UserId = testUser2.Id, Name = "Get haircut", Done = false, DateTo = DateOnly.Parse("2026-5-10") }
                    );
                    await context.SaveChangesAsync();
                }


            }



        }


    }
}
