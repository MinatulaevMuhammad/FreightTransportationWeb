using FreightTransportationWeb.Data.Enum;
using FreightTransportationWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Net;

namespace FreightTransportationWeb.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Orders.Any())
                {
                    context.Orders.AddRange(new List<Order>()
                    {
                        new Order()
                        {
                            Customer = new AppUser()
                            {
                                UserName = "Ben",
                                UserRole = Enum.UserRole.Customer,
                                Address = new AddressUser()
                                {
                                    House = "24",
                                    Street = "Street1",
                                    City = "City1"
                                }
                            },
                            Contractor = new AppUser()
                            {
                                UserName = "Max",
                                UserRole = Enum.UserRole.Contractor,
                                Address = new AddressUser()
                                {
                                    House = "65",
                                    Street = "Street2",
                                    City = "City2"
                                }
                            },
                            DeliveryAddress = new DeliveryAddress()
                            {
                                House = "2",
                                Street = "Street3",
                                City = "City3",
                                PhoneNumber = "89224202777"
                            },
                            Package = new Package()
                            {
                                Weight = 25,
                                Length = 10,
                                Width = 2,
                                Height = 6,
                                Description = "Very big package"
                            },
                            OrderStatus = Enum.OrderStatus.InProgress
                        },                
                    
                        new Order()
                        {
                            Customer = new AppUser()
                            {
                                UserName = "Mark",
                                UserRole = Enum.UserRole.Customer,
                                Address = new AddressUser()
                                {
                                    House = "1",
                                    Street = "Street4",
                                    City = "City4"
                                }
                            },
                            DeliveryAddress = new DeliveryAddress()
                            {
                                House = "2",
                                Street = "Street5",
                                City = "City5",
                                PhoneNumber = "89224202888"
                            },
                            Package = new Package()
                            {
                                Weight = 25,
                                Length = 10,
                                Width = 2,
                                Height = 6,
                                Description = "Very big package"
                            },
                            OrderStatus = Enum.OrderStatus.Created
                        }                       
                    });
                    context.SaveChanges();
                }  
            }

        }
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "teddysmithdeveloper@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "teddysmithdev",
                        UserRole = UserRole.Customer,
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new AddressUser()
                        {
                            House = "34",
                            Street = "Main St",
                            City = "Charlotte",
                            
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "app-user",
                        UserRole = UserRole.Customer,
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new AddressUser()
                        {
                            House = "34",
                            Street = "123 Main St",
                            City = "Charlotte",
                            
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
