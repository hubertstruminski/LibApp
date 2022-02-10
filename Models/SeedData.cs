using System;
using System.Linq;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibApp.Models
{
    public static class SeedData
    {
        private static Customer[] customers =
        {
            new Customer
                {
                    Id = 1,
                    Name = "Hubert Strumiński",
                    MembershipTypeId = 4,
                    HasNewsletterSubscribed = false,
                    Birthdate = new DateTime(1995, 12, 21),
                },
                new Customer
                {
                    Id = 2,
                    Name = "Andrzej Kowalski",
                    MembershipTypeId = 3,
                    HasNewsletterSubscribed = true,
                    Birthdate = new DateTime(2000, 3, 13),
                },
                new Customer
                {
                    Id = 3,
                    Name = "Katarzyna Marek",
                    MembershipTypeId = 2,
                    HasNewsletterSubscribed = true,
                    Birthdate = new DateTime(1992, 9, 7),
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
    }
}