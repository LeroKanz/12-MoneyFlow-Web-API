using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VZ.MoneyFlow.FR.API.Grpc;
using VZ.MoneyFlow.FR.Data.Data;
using VZ.MoneyFlow.FR.IData.IServices;
using VZ.MoneyFlow.FR.Models.Configuration;
using VZ.MoneyFlow.FR.Services;

namespace VZ.MoneyFlow.FR.API
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
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>()).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            }));
            services.Configure<FormRecognizerEntity>(Configuration.GetSection("FormRecognizer"));

            services.AddScoped<IFormRecognizerService, FormRecognizeService>();

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VZ.MoneyFlow.FormRecognizer", Version = "v1" });
            });

            services.AddGrpc();

            services.AddAutoMapper(typeof(AppMapper));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("ApiCorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VZ.MoneyFlow.FormRecognizer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcFormRecognizerService>();
            });
        }
    }
}
