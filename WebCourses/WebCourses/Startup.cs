using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCourses.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCourses.Models;

namespace WebCourses
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(roleManager, userManager);
        }

        private void CreateRoles(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            //var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "Admin", "Teacher", "Student" };
            Task<IdentityResult> roleResult;

            foreach (var roleName in roleNames)
            {
                Task<bool> roleExist = roleManager.RoleExistsAsync(roleName);
                roleExist.Wait();

                if (!roleExist.Result)
                {
                    roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
                    roleResult.Wait();
                }
            }

            Task<User> _user = userManager.FindByEmailAsync(Configuration["AdminEmail"]);
            _user.Wait();
            if (_user.Result == null)
            {
                var admin = new User
                {
                    UserName = Configuration["AdminEmail"],
                    Email = Configuration["AdminEmail"],
                };
                string adminPassword = Configuration["Password"];
                Task<IdentityResult> createAdmin = userManager.CreateAsync(admin, adminPassword);
                createAdmin.Wait();
                if (createAdmin.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(admin, "Admin");
                    newUserRole.Wait();
                }
            }

            _user = userManager.FindByEmailAsync(Configuration["TeacherEmail"]);
            _user.Wait();
            if (_user.Result == null)
            {
                var teacher = new User
                {
                    UserName = Configuration["TeacherEmail"],
                    Email = Configuration["TeacherEmail"],
                };
                string adminPassword = Configuration["Password"];
                Task<IdentityResult> createTeacher = userManager.CreateAsync(teacher, adminPassword);
                createTeacher.Wait();
                if (createTeacher.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(teacher, "Teacher");
                    newUserRole.Wait();
                }
            }

            _user = userManager.FindByEmailAsync(Configuration["StudentEmail"]);
            _user.Wait();
            if (_user.Result == null)
            {
                var student = new User
                {
                    UserName = Configuration["StudentEmail"],
                    Email = Configuration["StudentEmail"],
                };
                string studentPassword = Configuration["Password"];
                Task<IdentityResult> createStudent = userManager.CreateAsync(student, studentPassword);
                createStudent.Wait();
                if (createStudent.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(student, "Student");
                    newUserRole.Wait();
                }
            }

            _user = userManager.FindByEmailAsync(Configuration["Teacher1Email"]);
            _user.Wait();
            if (_user.Result == null)
            {
                var teacher = new User
                {
                    UserName = Configuration["Teacher1Email"],
                    Email = Configuration["Teacher1Email"],
                };
                string adminPassword = Configuration["Password"];
                Task<IdentityResult> createTeacher = userManager.CreateAsync(teacher, adminPassword);
                createTeacher.Wait();
                if (createTeacher.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(teacher, "Teacher");
                    newUserRole.Wait();
                }
            }

            _user = userManager.FindByEmailAsync(Configuration["Student2Email"]);
            _user.Wait();
            if (_user.Result == null)
            {
                var student = new User
                {
                    UserName = Configuration["Student2Email"],
                    Email = Configuration["Student2Email"],
                };
                string studentPassword = Configuration["Password"];
                Task<IdentityResult> createStudent = userManager.CreateAsync(student, studentPassword);
                createStudent.Wait();
                if (createStudent.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(student, "Student");
                    newUserRole.Wait();
                }
            }
        }
    }
}
