using Microsoft.AspNetCore.Mvc;

namespace prjWordAPI.Controllers
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

        //db/url class init
        dbControl dbc = new dbControl();
        //getReece method to start db class process
        [HttpGet("GetReece")]
        public void Getreece()
        {
            //get method
            dbc.getURL();
        }
        [HttpGet("GetSingle")]
        public String Getsingle(String Lang)
        {
            LangFactory langFactory = new LangFactory();
            ILang English = langFactory.getLang(Lang);
            word w = word.getInstance();
            return w.Single(English.getNames());
        }
        [HttpGet("GetAll")]
        public String[] GetAll(String Lang)
        {
            LangFactory langFactory = new LangFactory();
            ILang English = langFactory.getLang(Lang);
            word w = word.getInstance();
            return w.All(English.getNames());
        }
        [HttpGet("GetSorted")]
        public String[] GetSorted(String Lang)
        {
            LangFactory langFactory = new LangFactory();
            ILang English = langFactory.getLang(Lang);
            word w = word.getInstance();
            return w.Sorted(English.getNames());
        }
    }
}