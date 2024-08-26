using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ABREEProxy
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

                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(searchResult);
                });
            });
        }

        private async Task<string> ProxySearchRequest(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                // Fetch the search results using a search engine like Bing or DuckDuckGo
                var response = await client.GetAsync($"https://duckduckgo.com/?q={System.Net.WebUtility.UrlEncode(query)}");

                if (response.IsSuccessStatusCode)
                {
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
