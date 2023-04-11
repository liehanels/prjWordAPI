namespace prjWordAPI
{
    public class dbObject
    {
        public string First_Name;
        public string Last_Name;
        public string Password_Hash;
        public string Image_URL;

        public dbObject() 
        {
            this.First_Name = "";
            this.Last_Name = "";
            this.Password_Hash = "";
            this.Image_URL = "";
        }
        public dbObject(string first_Name, string last_Name, string password, string image_URL)
        {
            First_Name = first_Name;
            Last_Name = last_Name;
            Password_Hash = passHasher(password);
            Image_URL = image_URL;
        }
        private string passHasher(string password)
        {
            if (password == null) { return "Error : Password empty"; }
            else 
            {
                var hash = password.GetHashCode();
                return hash.ToString();
            }
        }
        public string summary()
        {
            string msg = "User :" + Last_Name + ", " + First_Name + "\n-> Image : " + Image_URL + "\nPassword Hash : " + Password_Hash;
            return msg;
        }
    }
}
