using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebProxy
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.SendFileAsync("wwwroot/index.html");
                });

                endpoints.MapPost("/search", async context =>
                {
                    var query = context.Request.Form["query"].ToString();

                    // Here, perform your search logic with the query.
                    // For example, you might search an external API or a local database.

                    // Just for demonstration, we'll return a simple message:
                    var result = $"You searched for: {query}";

                    await context.Response.WriteAsync(result);
                });
            });
        }
    }
}
