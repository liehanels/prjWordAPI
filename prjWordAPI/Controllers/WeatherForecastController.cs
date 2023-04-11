using Microsoft.AspNetCore.Mvc;

namespace prjWordAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        //getReece method to start db class process
        [HttpGet("GetReece")]
        public void GetReece()
        {
            dbControl dbc = new dbControl();
            dbc.urlRequest();
        }
        [HttpGet("GetSingle")]
        public String Getsingle(String search)
        {
            dbControl dbc = new dbControl();
            return dbc.readFromDB(search);
        }
        [HttpGet("GetAll")]
        public String[] GetAll()
        {
            string[] dbcAr = new string[1000];
            dbControl dbc = new dbControl();
            int i = 0;
            foreach (var item in dbc.readFromDB())
            {
                dbcAr[i] = item + "";
            }
            return dbcAr;
        }
        [HttpGet("GetSorted")]
        public String[] GetSorted(String colomn)
        {
            string[] dbcAr = new string[100];
            dbControl dbc = new dbControl();
            int i = 0;
            foreach (var item in dbc.readFromDBSorted(colomn))
            {
                dbcAr[i] = item + "";
                i++;
            }
            return dbcAr;
        }
    }
}