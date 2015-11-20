namespace ShuntingYard
{
    public interface ITokenizer
    {
        /// <summary>
        /// Initializes the Tokenizer.
        /// </summary>
        /// <param name="source">The source string to tokenize.</param>
        /// <param name="knownSymbols">A array of known symbols.</param>
        void Initialize(string source, char[] knownSymbols);

        /// <summary>
        /// Read the next available Token.
        /// Token.Type will be TokenType.None if no other token was found.
        /// </summary>
        /// <returns>The next Token that was read or a Token with Token.Type == TokenType.None if the end of the source string was reached.</returns>
        Token ReadNextToken();
    }
}