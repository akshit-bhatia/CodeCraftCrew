using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.SqlServer;

using Test1.Data;
using Microsoft.OpenApi.Models;

namespace Test1
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add serices to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddDbContext...
			services.AddControllers();
			//	services.AddDbContext<AppDbContext>(options =>
			//options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddDbContext<AppDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "CodeCraftCrew API", Version = "v0.2", Description = "University of Windsor, Business 6, Group 9" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(WebApplication app, IWebHostEnvironment env)
		{
			// Other configurations

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}