namespace ShuntingYardUnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using KPMG.DE.Traction.Business.EffectCalculation;

    [TestClass]
    public class InfixToPostfixConverterTest
    {
        [TestMethod]
        public void ConvertInfixToPostfix()
        {
            InfixToPostfixConverter i2p = new InfixToPostfixConverter(GetTokenizer());
            string result = i2p.Convert("-6/-(-1-5)+-1");

            Assert.AreEqual("6 m 1 m 5 - m / 1 m +", result);
        }

        private ITokenizer GetTokenizer()
        {
            // TODO: mocking 
            return new Tokenizer();
        }
    }
}
