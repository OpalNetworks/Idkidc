using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

public class SearchController : Controller
{
    private readonly HttpClient _httpClient;

    public SearchController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Query is required.");
        }

        var encodedQuery = System.Net.WebUtility.UrlEncode(query);
        var googleSearchUrl = $"https://www.google.com/search?q={encodedQuery}";

        var response = await _httpClient.GetAsync(googleSearchUrl);
        var content = await response.Content.ReadAsStringAsync();

        return Content(content, "text/html");
    }
}
