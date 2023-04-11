using Microsoft.AspNetCore.Mvc;

namespace prjWordAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDBControler : ControllerBase
    {
        private readonly ILogger<UserDBControler> _logger;

        public UserDBControler(ILogger<UserDBControler> logger)
        {
            _logger = logger;
        }
        //getReece method to start db class process
        [HttpGet("GetReece")]
        public String GetReece()
        {
            dbControl dbc = new dbControl();
            string output = dbc.urlRequest();
            return output;
        }
        [HttpGet("GetSingle")]
        public String Getsingle(String search)
        {
            dbControl dbc = new dbControl();
            string output = dbc.readFromDB(search);
            Console.WriteLine("\nGetSingle Request Results :\n" + output);
            return output;
        }
        [HttpGet("GetAll")]
        public String GetAll()
        {
            dbControl dbc = new dbControl();
            string output = dbc.readAllFromDB();
            Console.WriteLine("\nGetAll Request Results :\n" + output);
            return output;
        }
        [HttpGet("GetSorted")]
        public String GetSorted(String colomn)
        {
            dbControl dbc = new dbControl();
            string output = dbc.readFromDBSorted(colomn);
            Console.WriteLine("\nGetSorted Request Results :\n" + output);
            return output;
        }
        [HttpGet("GetPassHash")]
        public String GetPassHash(String First_Name, String Last_Name, String Password)
        {
            dbControl dbc = new dbControl();
            string output = dbc.readPassHash(First_Name, Last_Name, Password);
            Console.WriteLine("\nGetPassHash Request Results :\n" + output);
            return output;
        }
        [HttpGet("LoginVerification")]
        public bool LoginVerification(String First_Name, String Last_Name, String Password)
        {
            dbControl dbc = new dbControl();
            bool output = dbc.passHashMatch(First_Name, Last_Name, Password);
            Console.WriteLine("\nGetPassHash Request Results :\n" + output);
            return output;
        }
    }
}