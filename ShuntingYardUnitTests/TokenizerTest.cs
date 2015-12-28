namespace ShuntingYardUnitTests
{
    using ShuntingYard;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TokenizerTest
    {
        [TestMethod]
        public void TokenizingTest()
        {
            // Arrange
            int count = 0;
            string source = "10-2+3232*4/5.2()";
            string[] expectedTokens = {"10", "-", "2", "+", "3232", "*", "4", "/", "5.2", "(", ")"};
            Tokenizer tokenizer = new Tokenizer(source, new[] { '+', '-', '*', '/', '^', '(', ')' });
            
            // Act and assert each act.
            Token currentToken = tokenizer.ReadNextToken();
            while (currentToken.Type != TokenType.None)
            {
                Assert.AreEqual(expectedTokens[count], currentToken.GetValueAsString());
                currentToken = tokenizer.ReadNextToken();
                ++count;
            } 

            // Assert end result.
            Assert.AreEqual(expectedTokens.Length, count);
        }
    }
}
