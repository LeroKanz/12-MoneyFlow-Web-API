using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using VZ.MoneyFlow.API.Grpc;
using VZ.MoneyFlow.API.Middleware;
using VZ.MoneyFlow.EFData.Data;
using VZ.MoneyFlow.EFData.Repositories;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IRepositories;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models;
using VZ.MoneyFlow.Models.Models.Configuration;
using VZ.MoneyFlow.Services.Services;

namespace VZ.MoneyFlow.API
{
    public class FormRecognizerServiceFactory
    {
        private readonly Messaging _options;
        private readonly IServiceProvider _serviceProvider;

        public FormRecognizerServiceFactory(IOptions<Messaging> options, IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public IFormRecognizerService BuildService()
        {
            if (_options.MessageType == "grpc") 
                return _serviceProvider.GetRequiredService<FormRecognizeGrpcService>();

            if (_options.MessageType == "http") 
                return _serviceProvider.GetRequiredService<FormRecognizeService>();

            throw new NotImplementedException($"There are no implementation for type {_options.MessageType}.");
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>()).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            }));

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.Configure<FormRecognizer>(Configuration.GetSection("FormRecognizer"));
            services.Configure<Messaging>(Configuration.GetSection("Messaging"));

            services.AddHttpClient<IFormRecognizerService, FormRecognizeService>();
            FRBase.FRAPIBase = Configuration["ServiceUrls:FormRecognizer"];

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IAccountCurrencyService, AccountCurrencyService>();
            services.AddScoped<IAccountCurrencyRepository, AccountCurrencyRepository>();
            services.AddScoped<IOperationRepository, OperationRepository>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IExchangeRepository, ExchangeRepository>();
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<IAppUserAccountRepository, AppUserAccountRepository>();
            services.AddScoped<IAppUserAccountService, AppUserAccountService>();
            services.AddScoped<FormRecognizeService>();
            services.AddScoped<FormRecognizeGrpcService>();
            services.AddScoped<FormRecognizerServiceFactory>();

            services.AddScoped(s => s.GetRequiredService<FormRecognizerServiceFactory>().BuildService());

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VZ.MoneyFlow.API", Version = "v1" });
            });

            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });
            services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("LanguagePolicy", policy => policy.RequireClaim("language"));
            });

            services.AddAutoMapper(typeof(AppMapper));
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                if (context != null && context.Database != null)
                {
                    context.Database.Migrate();
                }
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("ApiCorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VZ.MoneyFlow.API v1"));
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();                
            });

            UpdateDatabase(app);
            AppDbInitializer.SeedData(app);
        }
    }
}
