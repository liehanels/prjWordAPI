namespace prjWordAPI
{
    public class dbObject
    {
        public string First_Name;
        public string Last_Name;
        public string Password_Hash;
        public string Image_URL;

        public dbObject(string first_Name, string last_Name, string password_Hash, string image_URL)
        {
            First_Name = first_Name;
            Last_Name = last_Name;
            Password_Hash = password_Hash;
            Image_URL = image_URL;
        }
    }
}
