namespace prjWordAPI
{
    public class Afrikaans : ILang
    {
        String[] arrWords = new String[10];
        public string[] getNames()
        {
            arrWords[0] = "Geselligheid";
            arrWords[1] = "Boerekos";
            arrWords[2] = "Lekker ";
            arrWords[3] = "Braai";
            arrWords[4] = "Trek";
            arrWords[5] = "Rooibos";
            arrWords[6] = "Bakkie";
            arrWords[7] = "Voetstoots";
            arrWords[8] = "Dop";
            arrWords[9] = "Kombuis";
            return arrWords;
        }
    }
    }
