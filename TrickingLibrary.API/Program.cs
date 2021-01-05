using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrickingLibrary.Data;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                if (env.IsDevelopment())
                {
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var testUser = new IdentityUser("test") { Email = "test@test.com" };
                    userMgr.CreateAsync(testUser, "password").GetAwaiter().GetResult();

                    var mod = new IdentityUser("mod") { Email = "mod@emai.com" };
                    userMgr.CreateAsync(mod, "password").GetAwaiter().GetResult();
                    userMgr.AddClaimAsync(mod, new Claim(TrickingLibraryConstants.Claims.Role, TrickingLibraryConstants.Roles.Mod))
                        .GetAwaiter()
                        .GetResult();

                    context.Add(new Difficulty { Slug = "easy", Active = true, Version = 1, Name = "Easy", Description = "Easy test" });
                    context.Add(new Difficulty { Slug = "medium", Active = true, Version = 1, Name = "Medium", Description = "medium test" });
                    context.Add(new Difficulty { Slug = "hard", Active = true, Version = 1, Name = "Hard", Description = "Hard test" });
                    context.Add(new Category { Slug = "kick", Active = true, Version = 1, Name = "Kick", Description = "Test" });
                    context.Add(new Category { Slug = "flip", Active = true, Version = 1, Name = "Flip", Description = "Test" });
                    context.Add(new Category { Slug = "swim", Active = true, Version = 1, Name = "Swim", Description = "Test" });
                    context.Add(new Trick
                    {
                        Slug = "backwards-roll",
                        Name = "Backwards Roll",
                        Active = true,
                        Version = 1,
                        Description = "This is Backwards Roll test",
                        Difficulty = "easy",
                        TrickCategories = new List<TrickCategory> {new TrickCategory{CategoryId = "flip"}}
                    });
                    context.Add(new Trick
                    {
                        Slug = "forwards-roll",
                        Name = "Forwards Roll",
                        Active = true,
                        Version = 1,
                        Description = "This is Forwards Roll test",
                        Difficulty = "easy",
                        TrickCategories = new List<TrickCategory> { new TrickCategory { CategoryId = "flip" } }
                    });
                    context.Add(new Trick
                    {
                        Slug = "back-flip",
                        Name = "Back Flip",
                        Active = true,
                        Version = 1,
                        Description = "This is Back Flip test",
                        Difficulty = "medium",
                        TrickCategories = new List<TrickCategory> { new TrickCategory { CategoryId = "flip" } },
                        Prerequisites = new List<TrickRelationship> { new TrickRelationship 
                            { 
                                PrerequisiteId = "backwards-roll"
                            } 
                        }
                    });
                    context.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "test descriptions",
                        Video = new Video
                        {
                            VideoLink = "https://localhost:5001/api/files/video/one.mp4",
                            ThumbLink = "https://localhost:5001/api/files/image/one.jpg"
                        },
                        VideoProcessed = true,
                        UserId = testUser.Id
                    });
                    context.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "test descriptions 2",
                        Video = new Video
                        {
                            VideoLink = "https://localhost:5001/api/files/video/two.mp4",
                            ThumbLink = "https://localhost:5001/api/files/image/two.jpg"
                        },
                        VideoProcessed = true,
                        UserId = testUser.Id
                    });
                    context.Add(new ModerationItem 
                    {
                        Target = "forwards-roll",
                        Type = ModerationTypes.Trick
                    });
                    context.SaveChanges();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
