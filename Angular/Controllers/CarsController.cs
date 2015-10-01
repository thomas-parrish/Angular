using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Angular.Models;
using Bing;
using Newtonsoft.Json;

namespace Angular.Controllers
{
    [RoutePrefix("api/cars")]
    public class CarsController : ApiController
    {
        readonly ApplicationDbContext _db = new ApplicationDbContext();

        public class Options
        {
            public string year { get; set; }
            public string make { get; set; }
            public string model { get; set; }
            public string trim { get; set; }
            public string filter { get; set; }
            public bool paging { get; set; }
            public int? page { get; set; }
            public int? perPage { get; set; }
        }

        [HttpPost]
        [Route("getYears")]
        public async Task<List<string>> GetYears()
        {
            return await _db.GetYears();
        }

        [HttpPost]
        [Route("getMakes")]
        public async Task<List<string>> GetMakes(Options options)
        {
            return await _db.GetMakes(options.year);
        }

        [HttpPost]
        [Route("getModels")]
        public async Task<List<string>> GetModels(Options options)
        {
            return await _db.GetModels(options.year, options.make);
        }

        [HttpPost]
        [Route("getTrims")]
        public async Task<List<string>> GetTrims(Options options)
        {
            return await _db.GetTrims(options.year, options.make, options.model);
        }

        [HttpPost]
        [Route("getCars")]
        public async Task<List<Car>> GetCars(Options options)
        {
            return await _db.GetCars(options.year, options.make, options.model, options.trim, options.filter, options.paging, options.page, options.perPage);
        }

        [HttpPost, Route("getCar")]
        public async Task<IHttpActionResult> GetCar([FromBody]int id)
        {
            var car = _db.Cars.Find(id);
            dynamic recalls = "";
            var image = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.nhtsa.gov/");
                try
                {
                    var response = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + car.model_year + "/make/" + car.make + "/model/" + car.model_name + "?format=json");
                    recalls = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }

            var images = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));
            images.Credentials = new NetworkCredential("accountKey", 
                ConfigurationManager.AppSettings["bing"]);
            var marketData = images.Composite(
                "image",
                car.model_year + " " + car.make + " " + car.model_name + " " + car.model_trim,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                ).Execute();

            image = marketData?.FirstOrDefault()?.Image.FirstOrDefault()?.MediaUrl;

            return Ok(new {car,image,recalls});
        }
    }
}
