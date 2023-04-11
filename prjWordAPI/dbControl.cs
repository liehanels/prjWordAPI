using System;
using System.Data;
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
        /* not now
          private List<string> afrL = new List<string>();
          private List<string> engL = new List<string>();
          private List<string> xhoL = new List<string>();
        */
        private const string connString = "Server=tcp:liehan-db.database.windows.net,1433;Initial Catalog=liehan-database;Persist Security Info=False;User ID=liehanels;Password=St10085345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private dbObject obj = new dbObject();

        //methods to make life easier
        //get Reece's URL responseFromServer off the website
        public string urlRequest()
        {
            string msg;
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
                    Console.WriteLine("Raw response from server\n\n" + responseFromServer + "\n\n" + addBorder((Console.BufferWidth / 2) - 10) + "<end of raw response>" + addBorder((Console.BufferWidth / 2) - 10) + "\n");
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
                    msg = "\nurlRquest() process has completed its actions successfully\n" + addBorder(Console.BufferWidth);
                }
            }
            catch (Exception ex)
            {
                msg = ex + addBorder(Console.BufferWidth);
            }
            return msg;
        }
        //inserts information into database
        public string insertToDB(int c)
        {
            string msg = "";
            SqlConnection sqlCon;
            try
            {
                using (sqlCon = new SqlConnection(connString))
                {
                    sqlCon.Open();
                    msg = ("\nConnection opened successfully\n" + addBorder(Console.BufferWidth));
                    Console.WriteLine(msg);
                    SqlCommand sqlCom;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    string sql = "INSERT INTO users_tbl values ('" + dbObjects[c].First_Name + "','" + dbObjects[c].Last_Name + "','" + dbObjects[c].Password_Hash + "','" + dbObjects[c].Image_URL + "')";
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
                msg =  ("\nRecord : " + dbObjects[c].summary() + "\nHas been added successfully\n" + addBorder(Console.BufferWidth));
            }
            catch (Exception e)
            {
                msg = "\nError : \n" + e + "\n" + addBorder(Console.BufferWidth);
            }
            return msg;
        }
        //generates the link and request
        public string URL_Link(int option)
        {
            const string url = "https://wordapidata.000webhostapp.com/";
            string link = url + req[option];
            return link;
        }
        //creates the request list
        public void addURLRequests()
        {
            req.Add("?getnamesenglish");  //[0]
            req.Add("?getnamesafrikaans");//[1]
            req.Add("?getnamesxhosa");    //[2]
            req.Add("?getuserdb");        //[3]
            Console.WriteLine("\nurl requests added successfully\n" + addBorder(Console.BufferWidth));
        }
        //takes the processed input and sorts it into a list of dbObjects
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
                Console.WriteLine("Record : " + c + " ->" + dbObjects[c].First_Name + "->" + dbObjects[c].Last_Name + "->" + dbObjects[c].Password_Hash + "->" + dbObjects[c].Image_URL + addBorder(Console.BufferWidth));
                c++;
            }
            Console.WriteLine("\nprocessInformation(string responseFromServer) has completed its actions successfully\n" + addBorder(Console.BufferWidth));
        }
        //gets the raw response from the server and formats it using string operators to simplify the data capture process
        public string processRawInput(string rawInput)
        {
            rawInput = rawInput.Replace("[", "");
            rawInput = rawInput.Replace("]", "");
            rawInput = rawInput.Replace("},{", "_");
            rawInput = rawInput.Replace("{", "");
            rawInput = rawInput.Replace("}", "");
            rawInput = rawInput.Replace("\"", "");
            rawInput = rawInput.Trim();
            Console.WriteLine("Modified response from server\n\n" + rawInput + "\n\n" + addBorder((Console.BufferWidth/2) - 10) + "<end of mod response>" + addBorder((Console.BufferWidth / 2) - 10) + "\n");
            Console.WriteLine("\nprocessRawInput(string rawInput) has completed its actions successfully\n" + addBorder(Console.BufferWidth));
            return rawInput;

        }
        //pretty GUI stuff
        public string addBorder(int totalSpaces)
        {
            string border = "";
            for (int i = 0; i < totalSpaces; i++)
            {
                border = border + "-";
            }
            return border;
        }
        //creates one dbObject and returns it
        public dbObject createDB_Oject(string fName, string lName, string pass, string imgURL)
        {
            obj = new dbObject(fName, lName, pass, imgURL);
            Console.WriteLine("\ncreateDB_Object() has completed its actions successfully\n" + addBorder(Console.BufferWidth));
            return obj;
        }
        //reads from db with feature to search by first name
        public string readFromDB(string search)
        {
            string msg = "";
            SqlConnection sqlCon;
            try
            {
                using (sqlCon = new SqlConnection(connString))
                {
                    sqlCon.Open();
                    msg = ("\nConnection opened successfully\n" + addBorder(Console.BufferWidth));
                    Console.WriteLine(msg);
                    SqlCommand sqlCom;
                    SqlDataReader reader;
                    string output = "";
                    string sql = "SELECT First_Name, Last_Name, Image_URL FROM users_tbl WHERE First_Name = '" + search + "'";
                    using (sqlCom = new SqlCommand(sql, sqlCon))
                    {
                        using (reader = sqlCom.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                output = output + "\nFirst_Name :\t" + reader.GetString(0) + "Last_Name :\t" + reader.GetString(1) + "\nImage_URL :\t" + reader.GetString(2) + "\n" + addBorder(Console.BufferWidth);
                            }
                            sqlCom.Dispose();
                            sqlCon.Close();
                        }
                    }
                    msg = (addBorder(Console.BufferWidth) + "\nResults\n" + addBorder(Console.BufferWidth) + output + "\n" + addBorder(Console.BufferWidth) + "\n" + addBorder(Console.BufferWidth) + "\n");
                }
            }
            catch (Exception e)
            {
                msg = "\nError : \n" + e + "\n" + addBorder(Console.BufferWidth);
            }
            return msg;
        }
        //reads all the data from the db
        public string readAllFromDB()
        {
            string msg = "";
            SqlConnection sqlCon;
            try
            {
                using (sqlCon = new SqlConnection(connString))
                {
                    sqlCon.Open();
                    msg = ("\nConnection opened successfully\n" + addBorder(Console.BufferWidth));
                    Console.WriteLine(msg);
                    SqlCommand sqlCom;
                    SqlDataReader reader;
                    string output = "";
                    string sql = "SELECT First_Name, Last_Name, Image_URL FROM users_tbl";
                    using (sqlCom = new SqlCommand(sql, sqlCon))
                    {
                        using (reader = sqlCom.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                output = output + "\nFirst_Name :\t" + reader.GetString(0) + "Last_Name :\t" + reader.GetString(1) + "\nImage_URL :\t" + reader.GetString(2) + "\n" + addBorder(Console.BufferWidth);
                            }
                            sqlCom.Dispose();
                            sqlCon.Close();
                        }
                    }
                    msg = (addBorder(Console.BufferWidth) + "\nResults\n" + addBorder(Console.BufferWidth) + output + "\n" + addBorder(Console.BufferWidth) + "\n" + addBorder(Console.BufferWidth) +"\n");
                }
            }
            catch (Exception e)
            {
                msg = "\nError : \n" + e + "\n" + addBorder(Console.BufferWidth);
            }
            return msg;
        }
        //reads all the data from the db sorted by a specified column
        public string readFromDBSorted(string column)
        {
            string msg = "";
            SqlConnection sqlCon;
            try
            {
                using (sqlCon = new SqlConnection(connString))
                {
                    sqlCon.Open();
                    msg = ("\nConnection opened successfully\n" + addBorder(Console.BufferWidth));
                    Console.WriteLine(msg);
                    SqlCommand sqlCom;
                    SqlDataReader reader;
                    string output = "";
                    string sql = "SELECT First_Name, Last_Name, Image_URL FROM users_tbl ORDER BY '" + column + "'";
                    using (sqlCom = new SqlCommand(sql, sqlCon))
                    {
                        using (reader = sqlCom.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                output = output + "\nFirst_Name :\t" + reader.GetString(0) + "Last_Name :\t" + reader.GetString(1) + "\nImage_URL :\t" + reader.GetString(2) + "\n" + addBorder(Console.BufferWidth);
                            }
                            sqlCom.Dispose();
                            sqlCon.Close();
                        }
                    }
                    msg = (addBorder(Console.BufferWidth) + "\nResults\n" + addBorder(Console.BufferWidth) + output + "\n" + addBorder(Console.BufferWidth) + "\n" + addBorder(Console.BufferWidth) + "\n");
                }
            }
            catch (Exception e)
            {
                msg = "\nError : \n" + e + "\n" + addBorder(Console.BufferWidth);
            }
            return msg;
        }
        //gets the password_hash
        public string readPassHash(string name, string lname, string Password)
        {
            string msg = "";
            string passHash = obj.passHasher(Password);
            SqlConnection sqlCon;
            try
            {
                using (sqlCon = new SqlConnection(connString))
                {
                    sqlCon.Open();
                    msg = ("\nConnection opened successfully\n" + addBorder(Console.BufferWidth));
                    Console.WriteLine(msg);
                    SqlCommand sqlCom;
                    SqlDataReader reader;
                    string output = "";
                    string sql = "SELECT First_Name, Last_Name, Password_Hash FROM users_tbl WHERE First_Name = '" + name + "' AND Password_Hash = '" + passHash + "'";
                    using (sqlCom = new SqlCommand(sql, sqlCon))
                    {
                        using (reader = sqlCom.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string f_name = reader.GetString(0).Trim();
                                string l_name = reader.GetString(1).Trim();
                                string passhash = reader.GetString(2).Trim();
                                Console.WriteLine(addBorder(Console.BufferWidth) + "\nF Name : " + f_name + "\nname : " + name + "\nL Name : " + l_name + "\nlname : " + lname + "\npasshash : " + passhash + "\npassHash : " + passHash);
                                if (name.ToLower().Trim() == f_name.ToLower().Trim() && l_name.ToLower().Trim() == lname.ToLower().Trim())// && passHash == passhash
                                {
                                    output = output + "\nFirst_Name :\t" + f_name + "\tLast_Name :\t" + l_name + "\nPassword_Hash :\t#" + passhash + "*\n" + addBorder(Console.BufferWidth);
                                }
                                else 
                                { 
                                    output = output + "\nInvalid username / password" + addBorder(Console.BufferWidth) + "\n";
                                    break;
                                }
                            }
                            sqlCom.Dispose();
                            sqlCon.Close();
                        }
                    }
                    msg = (addBorder(Console.BufferWidth) + "\nResults\n" + addBorder(Console.BufferWidth) + output + "\n" + addBorder(Console.BufferWidth) + "\n" + addBorder(Console.BufferWidth) + "\n");
                }
            }
            catch (Exception e)
            {
                msg = "\nError : \n" + e + "\n" + addBorder(Console.BufferWidth);
            }
            return msg;
        }
        public bool passHashMatch(string name, string lname, string Password)
        {
            Password = obj.passHasher(Password);
            string dbRead = readPassHash(name, lname, Password);
            //dbRead = dbRead.Substring(dbRead.IndexOf(':'));
            Console.WriteLine("dbRead : " + dbRead + "Password : " + Password);
            if(dbRead.Trim() == Password.Trim()) { return true; } else { return false; }
        }
    }
}
