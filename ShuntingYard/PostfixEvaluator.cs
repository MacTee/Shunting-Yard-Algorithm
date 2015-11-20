namespace ShuntingYard
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The PostfixEvaluator evaluates/calculates the result of a formula in postfix notation.
    /// </summary>
    public class PostfixEvaluator
    {
        #region public methods

        /// <summary>
        /// Evaluates/calculates the result of a formula in postfix notation.
        /// </summary>
        /// <param name="postfixString">The formula (in postfix notation) to evaluate.</param>
        /// <returns>The evaluated/calculated result of the formula as a double.</returns>
        public double Evaluate(string postfixString)
        {
            var stack = new Stack<double>();
            var input = new Queue<string>();

            foreach (var entry in postfixString.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                input.Enqueue(entry);

            while (input.Count > 0)
            {
                var entry = input.Dequeue();

                if (string.IsNullOrEmpty(entry))
                    continue;

                // - If the scanned character is an operand, add it to the stack.
                if (!IsOperator(entry))
                {
                    stack.Push(double.Parse(entry));
                }

                // if operator is a negation or a additional plus sign.
                else if (IsUnaryOperator(entry))
                {
                    var x = stack.Pop();
                    switch (entry)
                    {
                        case "m":
                            // if operator is a negation evaluate -1 * topstack and push the result on the stack.
                            stack.Push(x * -1);
                            break;
                        case "p":
                            // if operator is a additional plus sign just push topstack back on the stack.
                            stack.Push(x);
                            break;
                        default:
                            throw new Exception(string.Format("Unary operator \"{0}\" is unknown!", entry));
                    }
                }

                // If the scanned character is a binary operator, there will be at least two operands in the stack.
                else if (IsBinaryOperator(entry))
                {
                    if (stack.Count < 2)
                        throw new InvalidOperationException("Operator imbalance.");

                    //   - then we store the top most element of the stack(topStack) in a variable temp.
                    //     Pop the stack. Now evaluate topStack(Operator)temp. Let the result of this operation be retVal. 
                    //     Pop the stack and Push retVal into the stack.
                    double rightOperand = stack.Pop();
                    double leftOperand = stack.Pop();
                    switch (entry)
                    {
                        case "+":
                            stack.Push(leftOperand + rightOperand);
                            break;
                        case "-":
                            stack.Push(leftOperand - rightOperand);
                            break;
                        case "*":
                            stack.Push(leftOperand * rightOperand);
                            break;
                        case "/":
                            stack.Push(leftOperand / rightOperand);
                            break;
                        case "^":
                            stack.Push(Math.Pow(leftOperand, rightOperand));
                            break;
                    }
                }
                else
                {
                    throw new Exception("Unknown symbol!");
                }

                //   Repeat this step till all the characters are scanned.
            }

            // After all characters are scanned, we will have only one element in the stack. Return topStack.
            if (stack.Count > 1)
                throw new Exception("More than one result on the stack.");
            else if (stack.Count < 1)
                throw new Exception("No results on the stack.");

            return stack.Pop();
        }

        #endregion

        #region protected methods

        protected bool IsUnaryOperator(string entry)
        {
            return (entry == "m") || (entry == "p");
        }

        protected bool IsBinaryOperator(string entry)
        {
            return (entry == "+") || (entry == "-") || (entry == "*") || (entry == "/") || (entry == "^");
        }

        protected bool IsOperator(string entry)
        {
            return IsBinaryOperator(entry) || IsUnaryOperator(entry);
        }

        #endregion
    }
}
