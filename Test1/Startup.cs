using Microsoft.EntityFrameworkCore;

using Test1.Data;
using Microsoft.OpenApi.Models;
using Test1.Interface;
using Test1.Email;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

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
			services.AddCors();
			services.AddMvc();

			//services.AddDbContext...
			services.AddControllers();
			//	services.AddDbContext<AppDbContext>(options =>
			//options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddDbContext<AppDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "CodeCraftCrew API", Version = "v0.2", Description = "University of Windsor, Business 6, Group 9" });
			});

			services.AddTransient<IEmailSender, EmailSender>();

			//services.AddHangfire(configuration => configuration.UseSqlServerStorage("Hangfire"));
			//services.AddHangfireServer();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(WebApplication app, IWebHostEnvironment env)
		{
			// Other configurations

			//app.UseHangfireDashboard();
			//app.UseHangfireServer();

			app.UseCors(
				options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed((host) => true)
			);
			//app.UseMvc();
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}