using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebProxy
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Simple Web Proxy");
                });

                endpoints.MapPost("/search", async context =>
                {
                    var formData = context.Request.Form["query"];
                    var client = new HttpClient();

                    // Fetching the search results from a search engine (e.g., DuckDuckGo)
                    var response = await client.GetStringAsync($"https://duckduckgo.com/?q={formData}");
                    await context.Response.WriteAsync(response);
                });
            });
        }
    }
}
