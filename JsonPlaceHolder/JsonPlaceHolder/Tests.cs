using NUnit.Framework;

namespace JsonPlaceHolder
{
    public class Tests
    {

        [Test]
        public void Test1()
        {
            string checkAccountSetup = "hopefully I will use my personal account this time!";
            Assert.That(checkAccountSetup== "hopefully I will use my personal account this time!");
        }
    }
}