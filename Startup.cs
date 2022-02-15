using FluentValidation;
using FluentValidation.AspNetCore;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Middleware;
using LibApp.Models;
using LibApp.Models.Validators;
using LibApp.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibApp
{
    public class Startup
    {
        public static bool IsLoggedIn = false;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
                .AddJwtBearer(configuration =>
                {
                    configuration.RequireHttpsMetadata = false;
                    configuration.SaveToken = true;
                    configuration.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = authenticationSettings.JwtIssuer,
                        ValidAudience = authenticationSettings.JwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasBirthdate", builder => builder.RequireClaim("Birthdate"));
            });

            
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"), options => options.EnableRetryOnFailure()));

            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireUppercase = false;

            //    options.SignIn.RequireConfirmedEmail = false;
            //    options.SignIn.RequireConfirmedAccount = false;
            //    options.SignIn.RequireConfirmedPhoneNumber = false;
            //    options.Lockout.AllowedForNewUsers = false;
            //});

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
