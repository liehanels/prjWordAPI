namespace prjWordAPI
{
    public class English : ILang
    {
        String[] arrWords = new String[10];
        public string[] getNames()
        {
            arrWords[0] = "shell";
            arrWords[1] = "acceptable";
            arrWords[2] = "siege";
            arrWords[3] = "fair";
            arrWords[4] = "reflection";
            arrWords[5] = "bottle";
            arrWords[6] = "shadow";
            arrWords[7] = "tiger";
            arrWords[8] = "pill";
            arrWords[9] = "permission";
            return arrWords;
        }
    }
}
