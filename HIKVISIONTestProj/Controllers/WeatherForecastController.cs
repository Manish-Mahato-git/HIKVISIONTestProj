using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace HIKVISIONTestProj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        string baseUrl1 = "http://172.26.5.4/ISAPI/AccessControl";

        // Set the user name and password
        string username = "admin";
        string password = "nepal123";

        [HttpPost("GetAllAttendances")]
        public async Task<IActionResult> GetAllAttendancesAsync()
        {
            var url = "http://192.168.1.106/ISAPI/AccessControl/AcsEvent?format=json";

            // The JSON data to be sent in the request body
            var jsonData = @"
        {
            ""AcsEventCond"": {
                ""searchID"": ""1"",
                ""searchResultPosition"": 0,
                ""maxResults"": 30,
                ""major"": 0,
                ""minor"": 0,
                ""startTime"": ""2024-06-14T00:00:00+05:45"",
                ""endTime"": ""2024-06-14T23:59:59+05:45""
            }
        }";

            using (var httpClient = new HttpClient(new DigestHttpClientHandler(username, password)))
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                try
                {
                    var response = await httpClient.PostAsync(url, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    return Ok(responseString);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }


    }
}
