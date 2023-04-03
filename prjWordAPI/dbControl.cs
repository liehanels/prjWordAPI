using System;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.Xml;

namespace prjWordAPI
{
    public class dbControl
    {
        //declarations
        private static string url = "https://wordapidata.000webhostapp.com/";
        private List<string> req = new List<string>();
        private List<string> userL = new List<string>();
        private List<string> afrL = new List<string>();
        private List<string> engL = new List<string>();
        private List<string> xhoL = new List<string>();
        private string connString = "";
        private SqlConnection sqlCon;
        private dbObject obj;

        //get Reece's URL stuff off the website
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
                    /* performing a series of string operations to split the raw(r) response >> useful data and information
                     * raw data looks like >> [{"Name":"Chris Martin","Password":"football","imageURL":"https:\/\/picsum.photos\/200\/300"},{"Name":.../200\/300"},{...}...]
                     * removes square brackets */
                    responseFromServer = responseFromServer.Replace("[", " ");
                    responseFromServer = responseFromServer.Replace("]", " ");
                    responseFromServer = responseFromServer.Replace("},{", "_");
                    responseFromServer = responseFromServer.Replace("{", " ");
                    responseFromServer = responseFromServer.Replace("}", " ");
                    responseFromServer = responseFromServer.Trim();
                    /*
                     * data now looks like >> {"Name":"<>","Password":"<>","imageURL":"<>"_"":"","":"","":""_"":"","":"","":""_..._"":"","":"","":""}
                    writes the modified string because i wanna know what happens
                    */
                    Console.WriteLine("Modified response from server\n\n" + responseFromServer + "\n\n-------------<end of mod response>--------------\n");
                    /*
                     *
                     */
                    string[] stuff = responseFromServer.Split(new char[] {'_'});
                    int c = 0;
                    while (c < stuff.Length)
                    {
                        String[] temp = stuff[c].Split(new char[] {','});
                        int e = 0;
                        while(e < temp.Length)
                        {
                            userL.Add(c + " - " + temp[e].Trim());
                            e++;
                        }
                        c++;
                    }
                    int i = 0;
                    while (i < userL.Count)
                    {
                        Console.WriteLine("Record :\t" + userL[(i)] + " <end of " + (i) + ">\n" );
                        i++;
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
