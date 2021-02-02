using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjektiSMS.Areas.AdminPanel.Models;
using ProjektiSMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektiSMS
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                  .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "250440754134-jq2vss8b00jk0bibtbct34veje9o8u1j.apps.googleusercontent.com";
                options.ClientSecret = "6mToDAC_58KWEWBgbcU2tQac";
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
                options.SaveTokens = true;

                options.Events.OnCreatingTicket = ctx =>
                {
                    List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

                    tokens.Add(new AuthenticationToken()
                    {

                        Name = "TicketCreated",
                        Value = DateTime.UtcNow.ToString()
                    });

                    ctx.Properties.StoreTokens(tokens);

                    return Task.CompletedTask;
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("EditProductPolicy",
                    policy => policy.RequireClaim("Edit Product")
                    );
                options.AddPolicy("DeleteProductPolicy",
                    policy => policy.RequireClaim("Delete Product")
                    );
                options.AddPolicy("CreateProductPolicy",
                    policy => policy.RequireClaim("Create Product")
                    );
                options.AddPolicy("EditSellerPolicy",
                    policy => policy.RequireClaim("Edit Seller")
                    );
                options.AddPolicy("DeleteSellerPolicy",
                    policy => policy.RequireClaim("Delete Seller")
                    );
                options.AddPolicy("EditStockPolicy",
                    policy => policy.RequireClaim("Edit Stock")
                    );
                options.AddPolicy("DeleteStockPolicy",
                    policy => policy.RequireClaim("Delete Stock")
                    );
               
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
            //KETU LEJOHET THIRJJA E addHTTPCONTEXTACCSEORE PER DEPENDECY INJECTION
            services.AddHttpContextAccessor();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ElevatedRights", policy =>
                  policy.RequireRole("Admin", "User"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
               endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                 name: "userpanel_route", "UserPanel",
                 pattern: "UserPanel/{controller=Home}/{action=Dashboard}/{id?}");


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
