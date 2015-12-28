namespace ShuntingYard
{
    using System;
    using System.Linq;

    /// <summary>
    /// The Tokenizer separates the source string into individual Tokens (Symbols and integer/doubles).
    /// Symbols will be identified by the passed knownSymbols char array
    /// </summary>
    public class Tokenizer : ITokenizer
    {
        #region members

        protected char[] mKnownSymbols = new char[] { };
        protected int    mCurrentCharIndex = -1;
	    protected char   mCurrentChar = '\0';
	    protected char   mLastChar = '\0';
	    protected string mSource = string.Empty;
        protected string mTokenValueBuffer = string.Empty;

        #endregion

        #region ctors

        public Tokenizer()
        {
        }

        public Tokenizer(string source, char[] knownSymbols)
        {
            Initialize(source, knownSymbols);
        }

        #endregion

        #region public methods

        /// <summary>
        /// Initializes the Tokenizer.
        /// </summary>
        /// <param name="source">The source string to tokenize.</param>
        /// <param name="knownSymbols">A array of known symbols.</param>
        public void Initialize(string source, char[] knownSymbols)
        {
            mKnownSymbols = new char[] { };
            mCurrentCharIndex = -1;
            mCurrentChar = '\0';
            mLastChar = '\0';
            mSource = string.Empty;
            mTokenValueBuffer = string.Empty;

            if (source.Length == 0)
                throw new Exception("Empty or Invalid source!");
            if (knownSymbols.Length == 0)
                throw new Exception("Empty or Invalid known symbols!");

            mSource = source;
            mKnownSymbols = knownSymbols;
            mTokenValueBuffer = string.Empty;
            ReadNextChar();
        }

        /// <summary>
        /// Read the next available Token.
        /// Token.Type will be TokenType.None if no other token was found.
        /// </summary>
        /// <returns>The next Token that was read or a Token with Token.Type == TokenType.None if the end of the source string was reached.</returns>
        public Token ReadNextToken()
        {
	        mTokenValueBuffer = string.Empty;
	        SkipWhiteSpace();

	        if (EoF())
		        return new Token();

            if (IsDigit(mCurrentChar))
                return ReadDigits();

            return ReadSymbol();
        }
        
        #endregion

        #region protected methods

        protected void ReadNextChar()
        {
            mLastChar = mCurrentChar;
            mCurrentChar = mSource.Length > ++mCurrentCharIndex ? mSource[mCurrentCharIndex] : '\0';

            if (mSource.Length <= 0)
		        mCurrentChar = '\0';
        }

        protected void SkipWhiteSpace()
        {
	        while (mCurrentChar > '\0' && mCurrentChar <= ' ')
		        ReadNextChar();
        }

        protected void StoreAndReadNext()
        {
	        mTokenValueBuffer += mCurrentChar;
	        ReadNextChar();
        }

        protected string GetTokenValue()
        {
	        return mTokenValueBuffer;
        }

        protected bool EoF()
        {
	        return (mCurrentChar == '\0');
        }

        protected void CheckUnexpectedEoF()
        {
	        if (EoF())
		        throw new Exception("Unexpected end of source!");
        }

        protected void ThrowInvalidCharException()
        {
	        string msg = string.Empty;
	        if (mTokenValueBuffer.Length > 0)
                msg = string.Format("Invalid character {0} after {1} at position '{2}'.", mCurrentChar, mTokenValueBuffer, mCurrentCharIndex + 1);
	        else
                msg = string.Format("Invalid character {0} at position '{1}'.", mCurrentChar, mCurrentCharIndex + 1);

	        throw new Exception(msg);
        }

        protected bool IsDigit(char character)
        {
            return (character >= '0' && character <= '9');
        }

        protected bool IsNumericSeparator(char character)
        {
            return character == '.' || character == ',';
        }

        protected bool IsKnownSymbol(char character)
        {
            return mKnownSymbols.Contains(mCurrentChar);
        }

        protected Token ReadDigits()
        {
            do
            {
                StoreAndReadNext();
            }
            while (IsDigit(mCurrentChar) || (IsNumericSeparator(mCurrentChar) && !IsNumericSeparator(mLastChar)));

            var value = GetTokenValue();
            if (value.Contains(".") || value.Contains(","))
                return new Token(TokenType.Double, GetTokenValue());
            else
                return new Token(TokenType.Integer, GetTokenValue());
        }

        protected Token ReadSymbol()
        {
            if (IsKnownSymbol(mCurrentChar))
            {
                StoreAndReadNext();
                return new Token(TokenType.Symbol, GetTokenValue());
            }

            throw new Exception(string.Format("Unknown symbol '{0}' at position '{1}'.", mCurrentChar, mCurrentCharIndex + 1));
        }

        protected Token ReadString()
        {
            while (!EoF() && mCurrentChar != ' ' && !IsKnownSymbol(mCurrentChar))
                StoreAndReadNext();

            return new Token(TokenType.String, GetTokenValue());
        }

        #endregion
    }
}
