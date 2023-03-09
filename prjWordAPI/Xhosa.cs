namespace prjWordAPI
{
    public class Xhosa : ILang
    {
        string[] arrWords = new string[10];
        public string[] getNames()
        {
            arrWords[0] = "Amathole";
            arrWords[1] = "Ingqondo";
            arrWords[2] = "Isixhosa";
            arrWords[3] = "Umtata";
            arrWords[4] = "Umzimvubu";
            arrWords[5] = "Xhosa";
            arrWords[6] = "Ibhayi";
            arrWords[7] = "Icamagu";
            arrWords[8] = "Ukutya";
            arrWords[9] = "Iintsomi";

            return arrWords;
        }
    }
}
