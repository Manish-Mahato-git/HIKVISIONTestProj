using HIKVISIONTestProj.Serviceses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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


        string baseUrl = "http://192.168.10.85/ISAPI";

        string username = "admin";
        string password = "nepal123";

        [HttpPost("GetAllAttendances")]
        public async Task<IActionResult> GetAllAttendancesAsync()
        {
            var url = $"{baseUrl}/AccessControl/AcsEvent?format=json";

            // The JSON data to be sent in the request body test
            var jsonData = @"
                    {
                        ""AcsEventCond"": {
                            ""searchID"": ""1"",
                            ""searchResultPosition"": 0,
                            ""maxResults"": 30,
                            ""major"": 0,
                            ""minor"": 0,
                            ""startTime"": ""2024-08-14T00:00:00+05:45"",
                            ""endTime"": ""2024-08-14T23:59:59+05:45""
                        }
                    }";

            using (var httpClient = new HttpClient(new DigestHttpClientHandler(username, password)))
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                try
                {
                    var response = await httpClient.PostAsync(url, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    /*var responseString = @"{
	""AcsEvent"":	{
		""searchID"":	""1"",
		""totalMatches"":	11,
		""responseStatusStrg"":	""OK"",
		""numOfMatches"":	11,
		""InfoList"":	[{
				""major"":	2,
				""minor"":	1024,
				""time"":	""2024-08-14T10:17:47+05:45"",
				""type"":	0,
				""serialNo"":	1090,
				""currentVerifyMode"":	""invalid"",
				""mask"":	""unknown""
			}, {
				""major"":	5,
				""minor"":	22,
				""time"":	""2024-08-14T10:17:47+05:45"",
				""doorNo"":	1,
				""type"":	0,
				""serialNo"":	1091,
				""currentVerifyMode"":	""invalid"",
				""mask"":	""unknown""
			}, {
				""major"":	1,
				""minor"":	1028,
				""time"":	""2024-08-14T10:17:50+05:45"",
				""type"":	0,
				""serialNo"":	1092,
				""currentVerifyMode"":	""invalid"",
				""mask"":	""unknown""
			}, {
				""major"":	5,
				""minor"":	39,
				""time"":	""2024-08-14T10:18:16+05:45"",
				""cardType"":	1,
				""cardReaderNo"":	1,
				""doorNo"":	1,
				""type"":	0,
				""serialNo"":	1093,
				""currentVerifyMode"":	""fpOrface"",
				""mask"":	""unknown""
			}, {
				""major"":	2,
				""minor"":	1024,
				""time"":	""2024-08-14T10:35:23+05:45"",
				""type"":	0,
				""serialNo"":	1094,
				""currentVerifyMode"":	""invalid"",
				""mask"":	""unknown""
			}, {
				""major"":	5,
				""minor"":	22,
				""time"":	""2024-08-14T10:35:24+05:45"",
				""doorNo"":	1,
				""type"":	0,
				""serialNo"":	1095,
				""currentVerifyMode"":	""invalid"",
				""mask"":	""unknown""
			}, {
				""major"":	1,
				""minor"":	1028,
				""time"":	""2024-08-14T10:35:26+05:45"",
				""type"":	0,
				""serialNo"":	1096,
				""currentVerifyMode"":	""invalid"",
				""mask"":	""unknown""
			}, {
				""major"":	3,
				""minor"":	112,
				""time"":	""2024-08-14T11:21:54+05:45"",
				""remoteHostAddr"":	""192.168.10.76"",
				""type"":	0,
				""serialNo"":	1097,
				""currentVerifyMode"":	""invalid"",
				""mask"":	""unknown""
			}, {
				""major"":	5,
				""minor"":	21,
				""time"":	""2024-08-14T11:26:15+05:45"",
				""doorNo"":	1,
				""type"":	0,
				""serialNo"":	1098,
				""currentVerifyMode"":	""invalid"",
				""mask"":	""unknown""
			}, {
				""major"":	5,
				""minor"":	38,
				""time"":	""2024-08-14T11:26:15+05:45"",
				""cardType"":	1,
				""name"":	""Anishma"",
				""cardReaderNo"":	1,
				""doorNo"":	1,
				""employeeNoString"":	""1234"",
				""type"":	0,
				""serialNo"":	1099,
				""userType"":	""normal"",
				""currentVerifyMode"":	""fpOrface"",
				""attendanceStatus"":	""checkIn"",
				""label"":	""Check In"",
				""mask"":	""unknown""
			}, {
				""major"":	5,
				""minor"":	22,
				""time"":	""2024-08-14T11:26:20+05:45"",
				""doorNo"":	1,
				""type"":	0,
				""serialNo"":	1100,
				""currentVerifyMode"":	""invalid"",
				""mask"":	""unknown""
			}]
	}
}";*/
                    var val=JsonConvert.DeserializeObject<AcsEventResponse>(responseString);
                    return Ok(val);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }


    }
}
