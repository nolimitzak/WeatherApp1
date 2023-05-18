using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using WeatherApp1.Models;

namespace WeatherApp1.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {
            WeatherViewModel model = new WeatherViewModel();
            return View(model);
        }
        public IActionResult result()
        {
            WeatherViewModel model = new WeatherViewModel();
            return View(model);
        }
        public async Task<IActionResult> WeatherDetail(string City)
        {
            WeatherViewModel rslt = new WeatherViewModel();

            //Assign API KEY which received from OPENWEATHERMAP.ORG  
            string appId = "641f92e30d15403bef85fde3ac72760b";

            //API path with CITY parameter and other parameters.  
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", City, appId);

            using var httpClient = new HttpClient();

            // Make a request to the API endpoint
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON data
                var deserializedData = JsonSerializer.Deserialize<RootObject>(responseContent);

                // Assing the data from the response to the view model properties.


                rslt.Country = deserializedData.sys.country;
                rslt.City = deserializedData.name;
                rslt.Lat = Convert.ToString(deserializedData.coord.lat);
                rslt.Lon = Convert.ToString(deserializedData.coord.lon);
                rslt.Description = deserializedData.weather[0].description;
                rslt.Humidity = Convert.ToString(deserializedData.main.humidity);
                rslt.Temp = Convert.ToInt64(deserializedData.main.temp);
                rslt.TempFeelsLike = Convert.ToInt64(deserializedData.main.feels_like);
                rslt.TempMax = Convert.ToInt64(deserializedData.main.temp_max);
                rslt.TempMin = Convert.ToInt64(deserializedData.main.temp_min);
                rslt.WeatherIcon = deserializedData.weather[0].icon;

            }


            var jsonstring = JsonSerializer.Serialize(rslt);

            //Return JSON string.  
            return View("result", rslt);
        
        }
    }
}

