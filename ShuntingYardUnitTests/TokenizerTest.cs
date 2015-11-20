using System;
using ShuntingYard;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShuntingYardUnitTests
{
    [TestClass]
    public class TokenizerTest
    {
        [TestMethod]
        public void TokenizingTest()
        {
            string source = "10-2+3232*4/5.2()";
            string[] expectedTokens = new[] {"10", "-", "2", "+", "3232", "*", "4", "/", "5.2", "(", ")"};

            int count = 0;
            Token currentToken = null;
            Tokenizer tokenizer = new Tokenizer(source, new[] { '+', '-', '*', '/', '^', '(', ')' });
            currentToken = tokenizer.ReadNextToken();
            do
            {
                Assert.AreNotEqual(TokenType.None, currentToken.Type);
                Assert.AreEqual(expectedTokens[count], currentToken.GetValueAsString());
                ++count;
                currentToken = tokenizer.ReadNextToken();
            } while (currentToken.Type != TokenType.None);

            Assert.AreEqual(expectedTokens.Length, count);
        }
    }
}
