using prjWordAPI;

namespace TestWordAPI
{
    [TestClass]
    public class WordTest
    {
        word w = word.getInstance();
        

        [TestMethod]    
        public void TestOne()
        {
            Assert.IsNotNull(w.Single());
        }
        [TestMethod]
        public void TestSorted()
        {
            String[] expected = new string[10];
            expected[0] = "shell";
            expected[1] = "acceptable";
            expected[2] = "siege";
            expected[3] = "fair";
            expected[4] = "reflection";
            expected[5] = "bottle";
            expected[6] = "shadow";
            expected[7] = "tiger";
            expected[8] = "pill";
            expected[9] = "permission";


            CollectionAssert.AreEqual(expected.OrderBy(x=>x).ToArray(), w.Sorted());
        }
        [TestMethod]
        public void TestAll()
        {
            String[] expected = new string[10];
            expected[0] = "shell";
            expected[1] = "acceptable";
            expected[2] = "siege";
            expected[3] = "fair";
            expected[4] = "reflection";
            expected[5] = "bottle";
            expected[6] = "shadow";
            expected[7] = "tiger";
            expected[8] = "pill";
            expected[9] = "permission";


            CollectionAssert.AreEqual(expected, w.All());

        }
    }
}