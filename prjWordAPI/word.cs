using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace prjWordAPI
{
    public class word
    {
        
        private static word instance; 
         private word()
        {
            
        }
        public static word getInstance() 
        { 
            if (instance == null)
            {
                instance = new word();
            }
            return instance;
        }
        public String[] All(String[] arrWords)
        {
            return arrWords;
        }
        public String[] Sorted(String[] arrWords)
        {
            return arrWords.OrderBy(x => x).ToArray();
        }
        public String Single(String[] arrWords)
        {
            Random rnd = new Random();
            return arrWords[rnd.Next(arrWords.Length)];
        }
        

    }
}
