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
        private static string url = "https://wordapidata.000webhostapp.com/";
        private List<string> req = new List<string>();
        private List<dbObject> dbObjects = new List<dbObject>();
        private List<string> afrL = new List<string>();
        private List<string> engL = new List<string>();
        private List<string> xhoL = new List<string>();
        private string connString = "";
        private SqlConnection sqlCon;
        private dbObject obj;

        //get Reece's URL stuffFromServer off the website
        public void getURL()
        {
            //url queries
            req.Add("?getnamesenglish");  //[0]
            req.Add("?getnamesafrikaans");//[1]
            req.Add("?getnamesxhosa");    //[2]
            req.Add("?getuserdb");        //[3]
            //creates query request based on the option needed
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + req[3]);
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
                    responseFromServer = responseFromServer.Replace("[", "");
                    responseFromServer = responseFromServer.Replace("]", "");
                    responseFromServer = responseFromServer.Replace("},{", "_");
                    responseFromServer = responseFromServer.Replace("{", "");
                    responseFromServer = responseFromServer.Replace("}", "");
                    responseFromServer = responseFromServer.Replace("\"", "");
                    responseFromServer = responseFromServer.Trim();
                    /*
                     * data now looks like >> Name:<>,Password:<>,imageURL:<>_...
                    writes the modified string to console because i wanted to know what happened to it
                    */
                    Console.WriteLine("Modified response from server\n\n" + responseFromServer + "\n\n-------------<end of mod response>--------------\n");
                    /*
                     * now splits each record that i seperated by ' _ ' into an array and then further splits it into an object array
                     */
                    string[] stuffFromServer = responseFromServer.Split(new char[] {'_'});
                    int c = 0;
                    while (c < stuffFromServer.Length)
                    {
                        String[] temp = stuffFromServer[c].Split(new char[] {','});
                        string name = (temp[0].Substring(temp[0].IndexOf(':') + 1));
                        string fname = name.Substring(0, name.IndexOf(" ") + 1).Trim();
                        string lname = name.Substring(name.IndexOf(" ") + 1).Trim();
                        string password = (temp[1].Substring(temp[1].IndexOf(':') + 1)).Trim();
                        string imgURL = (temp[2].Substring(temp[2].IndexOf(':') + 1)).Trim();
                        //adds each object to array
                        dbObjects.Add(createDB_Oject(fname, lname, password, imgURL));
                        // i wanna see if it works
                        Console.WriteLine("Record : " + c + "->" + dbObjects[c].First_Name + "->" + dbObjects[c].Last_Name + "->" + dbObjects[c].Password_Hash + "->" + dbObjects[c].Image_URL);
                        c++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public dbObject createDB_Oject(string fName, string lName, string pass, string imgURL)
        {
            obj = new dbObject(fName, lName, pass, imgURL);
            return obj;
        }
        public void insertToDB()
        {
            connString = "";
            sqlCon = new SqlConnection(connString);
            sqlCon.Open();

        }
    }
}
