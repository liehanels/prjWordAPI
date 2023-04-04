using System;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace prjWordAPI
{
    public class dbControl
    {
        //declarations
        private List<string> req = new List<string>();
        private List<dbObject> dbObjects = new List<dbObject>();
        private List<string> afrL = new List<string>();
        private List<string> engL = new List<string>();
        private List<string> xhoL = new List<string>();
        private const string connString = "Server=tcp:liehan-db.database.windows.net,1433;Initial Catalog=liehan-database;Persist Security Info=False;User ID=liehanels;Password=St10085345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private dbObject obj;

        //get Reece's URL responseFromServer off the website
        public void urlRequest()
        {
            //adds url queries
            addURLRequests();
            //creates query request based on the option needed (note that comments explain for [3] (getuserdb) only)
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_Link(3));
            request.Method = "GET";
            request.Timeout = Timeout.Infinite;
            request.KeepAlive = true;
            long length = 0;
            //tries to get a response
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    //gets the response from the server as a string
                    string responseFromServer = reader.ReadToEnd();
                    //writes the raw response to console screen for visual verification/error checking
                    Console.WriteLine("Raw response from server\n\n" + responseFromServer + "\n\n-------------<end of raw response>--------------\n");
                    /* performing a series of (temporarily bad) string operations to split the raw response >> useful data and information
                     * raw data looks like >> [{"Name":"Chris Martin","Password":"football","imageURL":"https:\/\/picsum.photos\/200\/300"},{"Name":.../200\/300"},{...}...]
                     */
                    responseFromServer = processRawInput(responseFromServer);
                    /*
                     * data now looks like >> Name:<>,Password:<>,imageURL:<>_...
                     * writes the modified string to console because i wanted to know what happened to it
                     * now splits each record that i seperated by ' _ ' into an array and then further splits it into an object array
                     */
                    processInformation(responseFromServer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("\nurlRquest() process has completed its actions successfully\n");
        }
        //methods to make life easier
        public string insertToDB(int c)
        {
            string msg = "";
            SqlConnection sqlCon;
            try
            {
                using (sqlCon = new SqlConnection(connString))
                {
                    sqlCon.Open();
                    msg = ("\nConnection opened successfully\n");
                    SqlCommand sqlCom;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    string sql = "INSERT INTO liehan-database (" + dbObjects[c].First_Name + "," + dbObjects[c].Last_Name + "," + dbObjects[c].Password_Hash + "," + dbObjects[c].Image_URL;
                    using (sqlCom = new SqlCommand(sql, sqlCon))
                    {
                        using (adapter.InsertCommand = new SqlCommand(sql, sqlCon))
                        {
                            adapter.InsertCommand.ExecuteNonQuery();
                            sqlCom.Dispose();
                            sqlCon.Close();
                        }
                    }
                }
                msg =  ("\nObject " + c + " added successfully\n");
            }
            catch (Exception e)
            {
                msg = "\nError : \n" + e + "\n";
            }
            return msg;
        }
        public string URL_Link(int option)
        {
            const string url = "https://wordapidata.000webhostapp.com/";
            string link = url + req[option];
            return link;
        }
        public void addURLRequests()
        {
            req.Add("?getnamesenglish");  //[0]
            req.Add("?getnamesafrikaans");//[1]
            req.Add("?getnamesxhosa");    //[2]
            req.Add("?getuserdb");        //[3]
            Console.WriteLine("\nurl requests added successfully\n");
        }
        public void processInformation(String responseFromServer)
        {
            string[] stuffFromServer = responseFromServer.Split(new char[] { '_' });
            int c = 0;
            while (c < stuffFromServer.Length)
            {
                //splits information of each record
                String[] temp = stuffFromServer[c].Split(new char[] { ',' });
                string name = (temp[0].Substring(temp[0].IndexOf(':') + 1));
                string fname = name.Substring(0, name.IndexOf(" ") + 1).Trim();
                string lname = name.Substring(name.IndexOf(" ") + 1).Trim();
                string password = (temp[1].Substring(temp[1].IndexOf(':') + 1)).Trim();
                string imgURL = (temp[2].Substring(temp[2].IndexOf(':') + 1)).Trim();
                //adds each object to array
                dbObjects.Add(createDB_Oject(fname, lname, password, imgURL));
                //adds to database ?
                Console.WriteLine(insertToDB(c));
                // i wanna see if it works
                Console.WriteLine("Record : " + c + " ->" + dbObjects[c].First_Name + "->" + dbObjects[c].Last_Name + "->" + dbObjects[c].Password_Hash + "->" + dbObjects[c].Image_URL);
                c++;
            }
            Console.WriteLine("\nprocessInformation(string responseFromServer) has completed its actions successfully\n");
        }
        public string processRawInput(string rawInput)
        {
            rawInput = rawInput.Replace("[", "");
            rawInput = rawInput.Replace("]", "");
            rawInput = rawInput.Replace("},{", "_");
            rawInput = rawInput.Replace("{", "");
            rawInput = rawInput.Replace("}", "");
            rawInput = rawInput.Replace("\"", "");
            rawInput = rawInput.Trim();
            Console.WriteLine("Modified response from server\n\n" + rawInput + "\n\n-------------<end of mod response>--------------\n");
            Console.WriteLine("\nprocessRawInput(string rawInput) has completed its actions successfully\n");
            return rawInput;

        }
        public dbObject createDB_Oject(string fName, string lName, string pass, string imgURL)
        {
            obj = new dbObject(fName, lName, pass, imgURL);
            Console.WriteLine("\ncreateDB_Object() has completed its actions successfully\n");
            return obj;
        }
    }
}
