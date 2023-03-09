namespace prjWordAPI
{
    public class LangFactory
    {
        public ILang returnInstance;
        public ILang getLang(string name)
        {
            if(name.ToLower().Equals("afrikaans"))
            {
                return returnInstance = new Afrikaans();
            }
            else if(name.ToLower().Equals("xhosa"))
                {
                    return returnInstance= new Xhosa();
                }
            else  
            {
                return returnInstance = new English();
            }
        }
    }
}
