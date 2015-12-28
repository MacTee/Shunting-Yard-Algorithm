namespace ShuntingYardUnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ShuntingYard;

    [TestClass]
    public class InfixToPostfixConverterTest
    {
        [TestMethod]
        public void ConvertInfixToPostfix()
        {
            // Arrange
            string formula = "-6/-(-1-5)+-1";
            InfixToPostfixConverter i2p = new InfixToPostfixConverter(new Tokenizer());

            // Act
            string result = i2p.Convert(formula);

            // Assert
            Assert.AreEqual("6 m 1 m 5 - m / 1 m +", result, string.Format("Conversion of \"{0}\" failed!", formula));


            // Arrange
            formula = "-6/-(-1-5)++1";

            // Act
            result = i2p.Convert(formula);

            // Assert
            Assert.AreEqual("6 m 1 m 5 - m / 1 p +", result, string.Format("Conversion of \"{0}\" failed!", formula));


            // Arrange
            formula = "3 + 4 * 2 / (1 - 5) ^ 2 ^ 3";

            // Act
            result = i2p.Convert(formula);

            // Assert
            Assert.AreEqual("3 4 2 * 1 5 - 2 3 ^ ^ / +", result, string.Format("Conversion of \"{0}\" failed!", formula));


            // Arrange
            formula = "1+(1)+(1)+1";

            // Act
            result = i2p.Convert(formula);

            // Assert
            Assert.AreEqual("1 1 + 1 + 1 +", result, string.Format("Conversion of \"{0}\" failed!", formula));


            // Arrange
            formula = "1+(-1)+(-1)+1";

            // Act
            result = i2p.Convert(formula);

            // Assert
            Assert.AreEqual("1 1 m + 1 m + 1 +", result, string.Format("Conversion of \"{0}\" failed!", formula));


            // Arrange
            formula = "(1)-(-1)";

            // Act
            result = i2p.Convert(formula);

            // Assert
            Assert.AreEqual("1 1 m -", result, string.Format("Conversion of \"{0}\" failed!", formula));
        }
    }
}
