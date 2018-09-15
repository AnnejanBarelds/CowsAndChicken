using CowsAndChicken.ApplicationServices;
using CowsAndChicken.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace CowsAndChicken.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var contextOptions = new DbContextOptionsBuilder<CowsAndChickenContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CowsAndChicken;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;
            var context = new CowsAndChickenContext(contextOptions);
            context.Database.EnsureCreated();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Cows And Chicken", Version = "v1" });
            });

            services.AddDbContext<ICowsAndChickenContext, CowsAndChickenContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CowsAndChicken;Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddScoped<ICowsAndChickenGameService, CowsAndChickenGameService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cows And Chicken V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
