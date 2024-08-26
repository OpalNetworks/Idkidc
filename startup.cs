using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
                    var searchResult = await ProxySearchRequest(query);

                    // Return the search results directly as HTML content
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(searchResult);
                });
            });
        }

        private async Task<string> ProxySearchRequest(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                // Perform a GET request to a search engine (like Bing or Google)
                var response = await client.GetAsync($"https://www.bing.com/search?q={System.Net.WebUtility.UrlEncode(query)}");

                if (response.IsSuccessStatusCode)
                {
                    // Return the search results page as HTML
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return "<h1>Error fetching search results</h1>";
                }
            }
        }
    }
}
