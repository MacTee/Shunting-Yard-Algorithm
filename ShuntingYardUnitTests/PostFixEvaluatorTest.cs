namespace ShuntingYardUnitTests
{
    using ShuntingYard;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PostfixEvaluatorTest
    {
        [TestMethod]
        public void EvaluatePostFixString()
        {
            // Arrange
            string postFixString = " 6 m 1 m 5 - m / 1 m +  "; // == -6/-(-1-5)+-1 (infix)
            PostfixEvaluator pe = new PostfixEvaluator();

            // Act
            decimal result = pe.Evaluate(postFixString);

            // Assert
            Assert.AreEqual(-2m, result, string.Format("Evaluation of \"{0}\" failed!", postFixString));


            // Arrange
            postFixString = " 6 m 1 m 5 - m / 1 p +  "; // == -6/-(-1-5)++1 (infix)

            // Act
            result = pe.Evaluate(postFixString);

            // Assert
            Assert.AreEqual(0m, result, string.Format("Evaluation of \"{0}\" failed!", postFixString));


            // Arrange
            postFixString = "3 4 2 * 1 5 - 2 3 ^ ^ / +"; // 3 + 4 * 2 / (1 - 5) ^ 2 ^ 3 (infix)

            // Act
            result = pe.Evaluate(postFixString);

            // Assert
            Assert.AreEqual(3.0001220703125m, result, string.Format("Evaluation of \"{0}\" failed!", postFixString));


            // Arrange
            postFixString = "1 1 + 1 + 1 +"; // 1+(1)+(1)+1 (infix)

            // Act
            result = pe.Evaluate(postFixString);

            // Assert
            Assert.AreEqual(4m, result, string.Format("Evaluation of \"{0}\" failed!", postFixString));


            // Arrange
            postFixString = "1 1 m + 1 m + 1 +"; // 1+(-1)+(-1)+1 (infix)

            // Act
            result = pe.Evaluate(postFixString);

            // Assert
            Assert.AreEqual(0m, result, string.Format("Evaluation of \"{0}\" failed!", postFixString));


            // Arrange
            postFixString = "1 1 m -"; // (1)-(-1) (infix)

            // Act
            result = pe.Evaluate(postFixString);

            // Assert
            Assert.AreEqual(2m, result, string.Format("Evaluation of \"{0}\" failed!", postFixString));

            // 
        }
    }
}
