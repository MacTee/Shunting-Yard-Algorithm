using System;
using ShuntingYard;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShuntingYardUnitTests
{
    [TestClass]
    public class PostfixEvaluatorTest
    {
        [TestMethod]
        public void EvaluatePostFixString()
        {
            string postFixString = " 6 m 1 m 5 - m / 1 m +  "; // == -6/-(-1-5)+-1 (infix)
            PostfixEvaluator pe = new PostfixEvaluator();
            double result = pe.Evaluate(postFixString);

            Assert.AreEqual(-2, result);
        }
    }
}
