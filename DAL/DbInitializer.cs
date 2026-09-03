using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieBookingPro.Models;

namespace MovieBookingPro.DAL
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 1. Roles
            string[] roles = { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Admin Users
            string[] adminEmails = { "admin@movienest.com", "admin@cinebook.com" };
            const string adminPassword = "Admin@123";

            foreach (var email in adminEmails)
            {
                var adminUser = await userManager.FindByEmailAsync(email);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FullName = "System Administrator",
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
                else if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 3. Demo Customer User
            string[] customerEmails = { "customer@movienest.com", "customer@cinebook.com" };
            const string customerPassword = "Customer@123";

            foreach (var email in customerEmails)
            {
                var customerUser = await userManager.FindByEmailAsync(email);
                if (customerUser == null)
                {
                    customerUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FullName = "John Doe",
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(customerUser, customerPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(customerUser, "Customer");
                    }
                }
            }

            // 4. Movies
            if (!await context.Movies.AnyAsync())
            {
                var movies = new List<Movie>
                {
                    new Movie
                    {
                        Title = "Inception",
                        Genre = "Sci-Fi",
                        Language = "English",
                        DurationInMinutes = 148,
                        Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                        ReleaseDate = new DateTime(2024, 7, 16),
                        PosterUrl = "https://images.unsplash.com/photo-1534447677768-be436bb09401?auto=format&fit=crop&w=800&q=80"
                    },
                    new Movie
                    {
                        Title = "Interstellar",
                        Genre = "Sci-Fi",
                        Language = "English",
                        DurationInMinutes = 169,
                        Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                        ReleaseDate = new DateTime(2024, 11, 7),
                        PosterUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&w=800&q=80"
                    },
                    new Movie
                    {
                        Title = "The Dark Knight",
                        Genre = "Action",
                        Language = "English",
                        DurationInMinutes = 152,
                        Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                        ReleaseDate = new DateTime(2024, 7, 18),
                        PosterUrl = "https://images.unsplash.com/photo-1509198397868-475647b2a1e5?auto=format&fit=crop&w=800&q=80"
                    },
                    new Movie
                    {
                        Title = "Oppenheimer",
                        Genre = "Drama",
                        Language = "English",
                        DurationInMinutes = 180,
                        Description = "The story of American scientist J. Robert Oppenheimer and his role in the development of the atomic bomb during World War II.",
                        ReleaseDate = new DateTime(2024, 7, 21),
                        PosterUrl = "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?auto=format&fit=crop&w=800&q=80"
                    },
                    new Movie
                    {
                        Title = "Dune: Part Two",
                        Genre = "Sci-Fi",
                        Language = "English",
                        DurationInMinutes = 166,
                        Description = "Paul Atreides unites with Chani and the Fremen while seeking revenge against the conspirators who destroyed his family.",
                        ReleaseDate = new DateTime(2025, 3, 1),
                        PosterUrl = "https://images.unsplash.com/photo-1478760329108-5c3ed9d495a0?auto=format&fit=crop&w=800&q=80"
                    },
                    new Movie
                    {
                        Title = "Spider-Man: Across the Spider-Verse",
                        Genre = "Animation",
                        Language = "English",
                        DurationInMinutes = 140,
                        Description = "Miles Morales catapults across the Multiverse, where he encounters a team of Spider-People charged with protecting its very existence.",
                        ReleaseDate = new DateTime(2024, 6, 2),
                        PosterUrl = "https://images.unsplash.com/photo-1607604276583-eef5d076aa5f?auto=format&fit=crop&w=800&q=80"
                    }
                };

                await context.Movies.AddRangeAsync(movies);
                await context.SaveChangesAsync();
            }

            // 5. Theatres & Screens
            if (!await context.Theatres.AnyAsync())
            {
                var pvr = new Theatre
                {
                    Name = "PVR Icon Grand Mall",
                    Location = "Downtown Central, Floor 4",
                    Screens = new List<Screen>
                    {
                        new Screen { ScreenName = "IMAX 4K Laser", SeatCapacity = 150 },
                        new Screen { ScreenName = "Audi 1 Dolby Atmos", SeatCapacity = 100 },
                        new Screen { ScreenName = "Gold Class VIP", SeatCapacity = 50 }
                    }
                };

                var inox = new Theatre
                {
                    Name = "INOX Megaplex Cinema",
                    Location = "Riverside Promenade, Block B",
                    Screens = new List<Screen>
                    {
                        new Screen { ScreenName = "Screen 1 - 4DX", SeatCapacity = 120 },
                        new Screen { ScreenName = "Screen 2 - INSIGNIA", SeatCapacity = 60 },
                        new Screen { ScreenName = "Screen 3 - RealD 3D", SeatCapacity = 110 }
                    }
                };

                var cinepolis = new Theatre
                {
                    Name = "Cinepolis VIP Cinema",
                    Location = "Metro Square, Level 3",
                    Screens = new List<Screen>
                    {
                        new Screen { ScreenName = "Screen 1 - Macro XE", SeatCapacity = 140 },
                        new Screen { ScreenName = "Screen 2 - Junior Club", SeatCapacity = 80 }
                    }
                };

                await context.Theatres.AddRangeAsync(pvr, inox, cinepolis);
                await context.SaveChangesAsync();
            }

            // 6. Show Schedules (Ensure upcoming shows exist for all movies)
            if (!await context.ShowSchedules.AnyAsync(s => s.ShowDate >= DateTime.Today))
            {
                var allMovies = await context.Movies.ToListAsync();
                var allScreens = await context.Screens.Include(s => s.Theatre).ToListAsync();

                if (allMovies.Count > 0 && allScreens.Count > 0)
                {
                    var shows = new List<ShowSchedule>();
                    var today = DateTime.Today;

                    var times = new[]
                    {
                        new TimeSpan(10, 30, 0),
                        new TimeSpan(14, 0, 0),
                        new TimeSpan(17, 30, 0),
                        new TimeSpan(21, 0, 0)
                    };

                    var prices = new[] { 250m, 350m, 450m, 300m, 400m };

                    for (int dayOffset = 0; dayOffset <= 6; dayOffset++)
                    {
                        var showDate = today.AddDays(dayOffset);

                        for (int m = 0; m < allMovies.Count; m++)
                        {
                            var movie = allMovies[m];
                            
                            // Each movie gets scheduled across 2 to 3 distinct screens per day
                            for (int sIdx = 0; sIdx < 3; sIdx++)
                            {
                                var screen = allScreens[(m * 2 + sIdx) % allScreens.Count];
                                var time = times[(m + sIdx) % times.Length];
                                var price = prices[(m + sIdx) % prices.Length];

                                shows.Add(new ShowSchedule
                                {
                                    MovieId = movie.MovieId,
                                    ScreenId = screen.ScreenId,
                                    ShowDate = showDate,
                                    ShowTime = time,
                                    Price = price
                                });
                            }
                        }
                    }

                    await context.ShowSchedules.AddRangeAsync(shows);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
