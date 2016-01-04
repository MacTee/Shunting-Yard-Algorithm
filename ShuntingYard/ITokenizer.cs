namespace ShuntingYard
{
    public interface ITokenizer
    {
        /// <summary>
        /// Initializes the Tokenizer.
        /// </summary>
        /// <param name="source">The source string to tokenize.</param>
        /// <param name="knownSymbols">A array of known symbols.</param>
        ITokenizer Initialize(string source, char[] knownSymbols);

        /// <summary>
        /// Read the next available Token.
        /// Token.Type will be TokenType.None if no other token was found.
        /// </summary>
        /// <returns>The next Token that was read or a Token with Token.Type == TokenType.None if the end of the source string was reached.</returns>
        Token ReadNextToken();

        /// <summary>
        /// Reads all available Tokens (from the current position).
        /// </summary>
        /// <returns>A Token array with all available tokens.</returns>
        Token[] ReadAllTokens();
    }
}