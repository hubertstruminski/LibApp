using System;
using System.Collections.Generic;
using System.Linq;
using LibApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibApp.Models
{
    public static class SeedData
    {
        private static readonly IPasswordHasher<Customer> _passwordHasher = new PasswordHasher<Customer>();

        private static Role[] roles =
        {
            new Role
            {
                Id = 1,
                Name = "User"
            },
            new Role
            {
                Id = 2,
                Name = "StoreManager",
            },
            new Role
            {
                Id = 3,
                Name = "Owner",
            },
        };

        private static Customer[] customers =
        {
            new Customer
                {
                    Id = 1,
                    Name = "Hubert Strumiński",
                    MembershipTypeId = 4,
                    HasNewsletterSubscribed = false,
                    Birthdate = new DateTime(1995, 12, 21),
                    Email = "hubert.struminski@gmail.com",
                    RoleId = roles[0].Id,
                    
                },
                new Customer
                {
                    Id = 2,
                    Name = "Andrzej Kowalski",
                    MembershipTypeId = 3,
                    HasNewsletterSubscribed = true,
                    Birthdate = new DateTime(2000, 3, 13),
                    Email = "andrzej.kowalski@gmail.com",
                    RoleId = roles[1].Id,
                },
                new Customer
                {
                    Id = 3,
                    Name = "Katarzyna Marek",
                    MembershipTypeId = 2,
                    HasNewsletterSubscribed = true,
                    Birthdate = new DateTime(1992, 9, 7),
                    Email = "katarzyna.marek@gmail.com",
                    RoleId = roles[2].Id,
                },
        };

        private static Book[] books =
        {
            new Book
                {
                    Id = 1,
                    Name = "Harry Potter",
                    AuthorName = "J.K. Rowling",
                    GenreId = 1,
                    DateAdded = new DateTime(2021, 5, 21),
                    ReleaseDate = new DateTime(2012, 12, 12),
                    NumberInStock = 12,
                    NumberAvailable = 4,
                },
                new Book
                {
                    Id = 2,
                    Name = "Storm",
                    AuthorName = "Elizabeth Burn",
                    GenreId = 2,
                    DateAdded = new DateTime(2021, 1, 30),
                    ReleaseDate = new DateTime(2015, 10, 27),
                    NumberInStock = 7,
                    NumberAvailable = 7,
                },
                new Book
                {
                    Id = 3,
                    Name = "Programowanie w Java",
                    AuthorName = "Mateusz Kościuszkowski",
                    GenreId = 3,
                    DateAdded = new DateTime(2021, 11, 10),
                    ReleaseDate = new DateTime(2020, 9, 17),
                    NumberInStock = 3,
                    NumberAvailable = 3,
                },
        };

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var executionStrategy = context.Database.CreateExecutionStrategy();

                executionStrategy.Execute(() => {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        if(!context.Roles.Any())
                        {
                            AddRoles(context);
                        }

                        EnableIdentityInsert(context, "Roles");
                        context.SaveChanges();

                        DisableAllIdentityInsert(context);

                        if (!context.MembershipTypes.Any())
                        {
                            AddMembershipTypes(context);
                        }

                        if (!context.Customers.Any())
                        {
                            AddCustomers(context);
                        }

                        EnableIdentityInsert(context, "Customers");
                        context.SaveChanges();

                        DisableAllIdentityInsert(context);

                        if (!context.Genre.Any())
                        {
                            AddGenres(context);
                        }

                        if(!context.Books.Any())
                        {
                            AddBooks(context);
                        }

                        EnableIdentityInsert(context, "Books");
                        context.SaveChanges();

                        DisableAllIdentityInsert(context);

                        if (!context.Rentals.Any())
                        {
                            AddRentals(context);
                        }

                        EnableIdentityInsert(context, "Rentals");
                        context.SaveChanges();

                        DisableAllIdentityInsert(context);

                        DisableIdentityInsert(context, "Customers");
                        DisableIdentityInsert(context, "Books");

                        UpdateCustomerPassword(context);
                        transaction.Commit();
                    }
                });
            }
        }

        public static void EnableIdentityInsert(ApplicationDbContext context, string tableName)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo." + tableName + " ON;");
        }

        public static void DisableIdentityInsert(ApplicationDbContext context, string tableName)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo." + tableName + " OFF;");
        }

        public static void DisableAllIdentityInsert(ApplicationDbContext context)
        {
            DisableIdentityInsert(context, "Customers");
            DisableIdentityInsert(context, "Books");
            DisableIdentityInsert(context, "Rentals");
            DisableIdentityInsert(context, "Roles");
        }

        public static void AddMembershipTypes(ApplicationDbContext context)
        {
            context.MembershipTypes.AddRange(
                    new MembershipType
                    {
                        Id = 1,
                        Name = "Pay as You Go",
                        SignUpFee = 0,
                        DurationInMonths = 0,
                        DiscountRate = 0
                    },
                    new MembershipType
                    {
                        Id = 2,
                        Name = "Monthly",
                        SignUpFee = 30,
                        DurationInMonths = 1,
                        DiscountRate = 10
                    },
                    new MembershipType
                    {
                        Id = 3,
                        Name = "Quaterly",
                        SignUpFee = 90,
                        DurationInMonths = 3,
                        DiscountRate = 15
                    },
                    new MembershipType
                    {
                        Id = 4,
                        Name = "Yearly",
                        SignUpFee = 300,
                        DurationInMonths = 12,
                        DiscountRate = 20
                    });
        }

        public static void AddCustomers(ApplicationDbContext context)
        {
            context.Customers.AddRange(customers);
        }

        public static void AddRoles(ApplicationDbContext context)
        {
            context.Roles.AddRange(roles);
        }

        public static void AddGenres(ApplicationDbContext context)
        {
            context.Genre.AddRange(
                new Genre
                {
                    Id = 1,
                    Name = "Fantasy",
                },
                new Genre
                {
                    Id = 2,
                    Name = "Thriller",
                },
                new Genre
                {
                    Id = 3,
                    Name = "Science",
                }
            );
        }

        public static void AddBooks(ApplicationDbContext context)
        {
            context.Books.AddRange(books);
        }

        public static void AddRentals(ApplicationDbContext context)
        {
            context.Rentals.AddRange(
                new Rental
                {
                    Id = 1,
                    Customer = customers[1],
                    Book = books[0],
                    DateRented = new DateTime(2022, 2, 3),
                    DateReturned = null,
                },
                new Rental
                {
                    Id = 2,
                    Customer = customers[2],
                    Book = books[2],
                    DateRented = new DateTime(2022, 1, 29),
                    DateReturned = new DateTime(2022, 2, 3),
                },
                new Rental
                {
                    Id = 3,
                    Customer = customers[0],
                    Book = books[1],
                    DateRented = DateTime.Now,
                    DateReturned = null,
                }
            );
        }

        public static void UpdateCustomerPassword(ApplicationDbContext context)
        {
            List<Customer> customers = context.Customers.ToList();

            foreach(var customer in customers)
            {
                customer.PasswordHash = _passwordHasher.HashPassword(customer, "1234567890");

                context.Update(customer);
                context.SaveChanges();
            }
        }
    }
}