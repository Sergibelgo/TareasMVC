using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Tutorial2TareasMVC.DBContext;
using Microsoft.AspNetCore.Mvc.Razor;
using Tutorial2TareasMVC.Services;

namespace Tutorial2TareasMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var politicaUsuariosAutenticados = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            builder.Services.AddDbContext<ContextDB>(opciones => opciones.UseSqlServer("name=DefaultConnection"));
            //builder.Services.AddDbContext<ContextDB>(opciones =>
            //opciones.UseMySql(
            //    builder.Configuration.GetConnectionString("MariaDbConnectionString"),
            //    new MariaDbServerVersion(new Version(10, 3, 27))
            //    ));

            // Add services to the container.
            builder.Services.AddControllersWithViews(opciones =>
            {
                opciones.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados));
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization(opciones =>
            {
                opciones.DataAnnotationLocalizerProvider = (_, factoria) => factoria.Create(typeof(RecursoCompartido));
            }); 
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IContextDB, ContextDB>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddAuthentication();

            //Añadir localicacion
            builder.Services.AddLocalization(opciones =>
            {
                opciones.ResourcesPath = "Resources";
            });

            //Añadir identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
            {
                opciones.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<ContextDB>()
            .AddDefaultTokenProviders();
            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
            {
                opciones.LoginPath = "/usuarios/login";
                opciones.AccessDeniedPath = "/usuarios/login";
            });
            var app = builder.Build();

            //Añadir mas culturas soportadas

            app.UseRequestLocalization(opciones =>
            {
                opciones.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("es");
                opciones.SupportedCultures = Constantes.culturasUISoportadas.Select(cultura=>new System.Globalization.CultureInfo(cultura.Value)).ToList();
                opciones.SupportedUICultures = Constantes.culturasUISoportadas.Select(cultura => new System.Globalization.CultureInfo(cultura.Value)).ToList();
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}