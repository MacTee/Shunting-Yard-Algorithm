namespace ShuntingYard
{
    /// <summary>
    /// Possible typs of Tokens
    /// </summary>
    public enum TokenType
    {
        None,
        Integer,
        Double,
        Symbol,
        String
    }

    /// <summary>
    /// Representation of a symbol or integer/double
    /// </summary>
    public class Token
    {
        #region members

        protected string mValue;

        #endregion

        #region properties

        public TokenType Type { get; protected set; }

        #endregion

        #region ctors

        public Token()
        {
            Type = TokenType.None;
            mValue = string.Empty;
        }

        public Token(TokenType type, string value)
        {
            Type = type;
            mValue = value;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Get the TokenType as string.
        /// </summary>
        /// <returns>The Type of the Token as string.</returns>
        public string   GetTypeAsString()
        { 
            switch (Type) 
            { 
                default:      
                    return "None";
                
                case TokenType.Integer:
                    return "Integer";

                case TokenType.Double:
                    return "Decimal";
                
                case TokenType.Symbol:
                    return "Symbol";

                case TokenType.String:
                    return "String";
            }
        }

        /// <summary>
        /// Get the value as integer.
        /// </summary>
        /// <returns>The value as a integer.</returns>
        public int      GetValueAsInt()
        {
            return int.Parse(mValue);
        }

        /// <summary>
        /// Get the value as a string.
        /// </summary>
        /// <returns>The value as a string.</returns>
        public string   GetValueAsString()
        {
            return mValue;
        }

        /// <summary>
        /// Get the value as a double.
        /// </summary>
        /// <returns>The value as a string.</returns>
        public double   GetValueAsDouble()
        {
            return double.Parse(mValue);
        }

        /// <summary>
        /// Compares the token with the passed type and value for equality.
        /// The tokens is equal when passed type and value are matching.
        /// </summary>
        /// <param name="type">Token Type that should be compared.</param>
        /// <param name="value">Token Value that should be compared.</param>
        /// <returns>True, when the passed type and value are matching to the Tokens type and value.</returns>
        public bool     Equals(TokenType type, string value)
        {
            return (Type == type && mValue == value);
        }

        #endregion
    }
}