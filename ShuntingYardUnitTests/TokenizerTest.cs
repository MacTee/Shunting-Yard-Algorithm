namespace ShuntingYardUnitTests
{
    using ShuntingYard;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TokenizerTest
    {
        [TestMethod]
        public void TokenizingTest_ReadNextToken()
        {
            // Arrange
            int count = 0;
            string[] expectedTokens = {"10", "-", "2", "+", "3232", "*", "4", "/", "5.2", "(", ")"};
            string source = string.Join(string.Empty, expectedTokens); //  = "10-2+3232*4/5.2()"
            ITokenizer tokenizer = new Tokenizer(source, new[] { '+', '-', '*', '/', '^', '(', ')' });
            
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

        [TestMethod]
        public void TokenizingTest_ReadAllTokens()
        {
            // Arrange
            int index = 0;
            string[] expectedTokens = { "10", "-", "2", "+", "3232", "*", "4", "/", "5.2", "(", ")" };
            string source = string.Join(string.Empty, expectedTokens); //  = "10-2+3232*4/5.2()"
            ITokenizer tokenizer = new Tokenizer().Initialize(source, new[] { '+', '-', '*', '/', '^', '(', ')' });

            // Act and assert each act.
            Token[] allTokens = tokenizer.ReadAllTokens();
            foreach (var token in allTokens)
            {
                Assert.AreEqual(expectedTokens[index++], token.GetValueAsString());
            }

            // Assert end result.
            Assert.AreEqual(expectedTokens.Length, index);
        }
    }
}
