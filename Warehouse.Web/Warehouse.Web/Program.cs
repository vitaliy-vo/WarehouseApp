using Mapster;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Warehouse.BLL;
using Warehouse.Core;
using Warehouse.Core.IRepositories;
using Warehouse.DAL;
using Warehouse.Web.Components;


namespace Warehouse.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();
            
            // Увеличиваем максимальный размер для загрузки файлов
            builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // 100 MB
            });

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            
            
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddScoped<IBlankRepository, BlankRepository>();
            builder.Services.AddScoped<IWarehousRepository, WarehousRepository>();
            builder.Services.AddScoped<IWarehouseTransactionRepository, WarehouseTransactionRepository>();
            builder.Services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();
            builder.Services.AddScoped<IDailyBalanceReportService, DailyBalanceReportService>();
            builder.Services.AddScoped<IDailyBalanceReportRepository, DailyBalanceReportRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMonthlyBalanceReportRepository, MonthlyBalanceReportRepository>();
            builder.Services.AddScoped<IMonthlyBalanceReportService, MonthlyBalanceReportService>();

            builder.Services.AddScoped<HttpContextAccessor>();
            builder.Services.AddScoped<BlankServise>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<WarehousServise>();
            builder.Services.AddScoped<WarehouseTransactionService>();
            builder.Services.AddScoped<TransactionTypeService>();
            builder.Services.AddScoped<DetailedBalanceReportService>();
            

            builder.Services.AddScoped<FileUploadService>();

            TypeAdapterConfig.GlobalSettings.Apply(new MapsterConfig());
            builder.Services.AddMapster();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                options =>
                {
                    options.Cookie.Name = "auth_token";
                    options.LoginPath = "/login";                 
                    options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
                });
            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddHttpContextAccessor();


            var app = builder.Build();
            IConfiguration config = app.Configuration;

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
            app.UseStatusCodePagesWithReExecute("/404");
            app.UseAntiforgery();
            app.Run();
        }
    }
}
