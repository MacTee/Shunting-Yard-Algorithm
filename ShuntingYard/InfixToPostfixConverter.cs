namespace ShuntingYard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The InfixToPostfixConverter converts a formula from infix notation to postfix notation.
    /// </summary>
    public class InfixToPostfixConverter
    {
        #region enumerators

        /// <summary>
        /// Enum to determine the associativity of a operation.
        /// </summary>
        public enum Associativity
        {
            Left,
            Right
        }

        #endregion

        #region members

        private readonly Operation[] Operations = new Operation[]
                                            {
                                                new Operation('+', 2, Associativity.Left), 
                                                new Operation('-', 2, Associativity.Left),
                                                new Operation('*', 3, Associativity.Left),
                                                new Operation('/', 3, Associativity.Left),
                                                new Operation('^', 4, Associativity.Right),
                                                new Operation('p', 4, Associativity.Right),
                                                new Operation('m', 4, Associativity.Right)
                                            };
        private readonly List<char> knownSymbols = new List<char>() { '(', ')' };

        private readonly ITokenizer myTokenizer = null;

        private Stack<string> operatorStack = new Stack<string>();
        private Queue<string> output = new Queue<string>();

        #endregion

        #region ctors

        public InfixToPostfixConverter(ITokenizer tokenizer)
        {
            myTokenizer = tokenizer;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Convert a formula from infix notation to postfix notation.
        /// </summary>
        /// <param name="infix">The source formula in infox notation.</param>
        /// <returns>A string with the passed formula in postfix notation.</returns>
        public string Convert(string infix)
        {
            operatorStack.Clear();
            output.Clear();

            knownSymbols.AddRange(Operations.Where(x => !knownSymbols.Contains(x.Symbol)).Select(x => x.Symbol).ToList());
            myTokenizer.Initialize(infix, knownSymbols.ToArray());

            return DoShuntingYardAlgorithm(myTokenizer).Trim();
        }

        #endregion

        #region protected methods

        protected string DoShuntingYardAlgorithm(ITokenizer tokenizer)
        {
            // see https://en.wikipedia.org/wiki/Shunting-yard_algorithm for details.

            // - Read a token.
            Token token = tokenizer.ReadNextToken();
            Token lastToken = null;
            bool first = true;
            bool afterOpenParenthesis = false;
            while (token.Type != TokenType.None)
            {
                // - If the token is a number, then add it to the output queue.
                if (IsDigit(token))
                    AddToOutput(token.GetValueAsString());

                // - If the token is a left parenthesis (i.e. "("), then push it onto the stack.
                else if (token.GetValueAsString() == "(")
                    operatorStack.Push(token.GetValueAsString());

                // - If the token is a right parenthesis (i.e. ")"):
                else if (token.GetValueAsString() == ")")
                {
                    //   - Until the token at the top of the stack is a left parenthesis, pop operators off the stack onto the output queue.
                    string lastOperation = operatorStack.Peek();
                    while (lastOperation != "(")
                    {
                        AddToOutput(operatorStack.Pop());
                        lastOperation = operatorStack.Peek();
                    }

                    //   - If the stack runs out without finding a left parenthesis, then there are mismatched parenthesis.
                    if (operatorStack.Count == 0)
                        throw new Exception("Invalid Syntax! - Parenthesis mismatch, missing open parenthesis");

                    //   - Pop the left parenthesis from the stack, but not onto the output queue.
                    operatorStack.Pop();

                    // Mark parenthesis flag to avoid false unary operation detection.
                    afterOpenParenthesis = true;
                }

                // - If the token is an operator, o1, then:
                else if (IsOperator(token))
                {
                    string currentOperation = token.GetValueAsString();

                    // check for negation at the beginning of the input formal.
                    if (first && currentOperation == "-")
                    {
                        currentOperation = "m";
                    }
                    // check for plus sign at the beginning of the input formal.
                    else if (first && currentOperation == "+")
                    {
                        currentOperation = "p";
                    }
                    else if (operatorStack.Count > 0)
                    {
                        string lastOperation = operatorStack.Peek();

                        // If parenthesis flag is set avoid false unary operation detection
                        if (!afterOpenParenthesis)
                        {
                            // check for negation direct after a '(' or after another operator.
                            if ((IsOperator(lastOperation) || lastOperation == "(") && currentOperation == "-" && lastOperation != "p" && lastOperation != "m" && !IsDigit(lastToken))
                            {
                                currentOperation = "m";
                            }

                            // check for plus sign direct after a '(' or after another operator.
                            if ((IsOperator(lastOperation) || lastOperation == "(") && currentOperation == "+" && lastOperation != "p" && lastOperation != "m" && !IsDigit(lastToken))
                            {
                                currentOperation = "p";
                            }
                        }

                        //   - while there is an operator token, lastOperation, at the top of the operator stack, and either
                        while (IsOperator(lastOperation))
                        {
                            //     o1 is left-associative and its precedence is less than or equal to that of lastOperation, or
                            //     o1 is right associative, and has precedence less than that of lastOperation,
                            if (IsLeftAssociative(currentOperation) && LessOrEqualPrecedence(currentOperation, lastOperation) ||
                                IsRightAssociative(currentOperation) && LessPrecedence(currentOperation, lastOperation))
                            {
                                //     then pop o2 off the operator stack, onto the output queue;
                                AddToOutput(operatorStack.Pop());

                                if (operatorStack.Count == 0)
                                    break;
                                else
                                    lastOperation = operatorStack.Peek();
                            }
                            else
                                break;
                        }
                    }
                        
                    //   - push o1 onto the operator stack.
                    operatorStack.Push(currentOperation);

                    // Reset parenthesis flag.
                    afterOpenParenthesis = false;
                }
                else
                {
                    throw new Exception(string.Format("Invalid Syntax! Unknown symbol \"{0}\"", token.GetValueAsString()));
                }
                
                lastToken = token;
                token = tokenizer.ReadNextToken();
                first = false;
            }

            // - When there are no more tokens to read:
            // - While there are still operator tokens in the stack:
            while (operatorStack.Count > 0)
            {
                string lastOperation = operatorStack.Peek();
                //   - If the operator token on the top of the stack is a parenthesis, then there are mismatched parenthesis.
                if (lastOperation == "(" || lastOperation == ")")
                    throw new Exception("Parenthesis mismatch! - Missing closing parenthesis");
                else
                    //   - Pop the operator onto the output queue.
                    AddToOutput(operatorStack.Pop());
            }

            return CreateOutput(output);
        }

        protected bool IsOperator(Token token)
        {
            return (token.Type == TokenType.Symbol && IsOperator(token.GetValueAsString()));
        }

        protected bool IsOperator(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                return false;

            return Operations.Any(x => x.Symbol == symbol[0]);
        }

        protected bool IsDigit(Token token)
        {
            return token != null && (token.Type == TokenType.Integer || token.Type == TokenType.Double);
        }

        protected void AddToOutput(string text)
        {
            output.Enqueue(" ");
            output.Enqueue(text);
        }

        protected bool LessPrecedence(string currentOperationSymbol, string lastOperationSymbol)
        {
            var o1 = GetOperation(currentOperationSymbol);
            var o2 = GetOperation(lastOperationSymbol);
            if (o1 == null || o2 == null)
                return false;

            return o1.Precedence < o2.Precedence;
        }

        protected bool LessOrEqualPrecedence(string currentOperationSymbol, string lastOperationSymbol)
        {
            var o1 = GetOperation(currentOperationSymbol);
            var o2 = GetOperation(lastOperationSymbol);
            if (o1 == null || o2 == null)
                return false;

            return o1.Precedence <= o2.Precedence;
        }

        protected bool IsLeftAssociative(string operationSymbol)
        {
            var o1 = GetOperation(operationSymbol);
            if (o1 == null)
                return false;

            return o1.Associativity == Associativity.Left;
        }

        protected bool IsRightAssociative(string operationSymbol)
        {
            var o1 = GetOperation(operationSymbol);
            if (o1 == null)
                return false;

            return o1.Associativity == Associativity.Right;
        }

        protected Operation GetOperation(string operationSymbol)
        {
            return Operations.FirstOrDefault(x => x.Symbol == operationSymbol[0]);
        }

        protected string CreateOutput(Queue<string> output)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var entry in output)
                sb.Append(entry);

            return sb.ToString();
        }

        #endregion

        #region internal classes

        public class Operation
        {
            public char Symbol;
            public int Precedence;
            public Associativity Associativity;

            public Operation(char symbol, int precendence, Associativity associativity)
            {
                this.Symbol = symbol;
                this.Precedence = precendence;
                this.Associativity = associativity;
            }
        }

        #endregion
    }
}
