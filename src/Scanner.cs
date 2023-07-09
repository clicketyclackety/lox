class Scanner
{
    readonly string source;
    readonly List<Token> tokens;
    private int start = 0;
    private int current = 0;
    private int line = 1;

    static readonly Dictionary<string, TokenType> keywords = new()
    {
        { "and", TokenType.AND },
        { "class", TokenType.CLASS },
        { "else", TokenType.ELSE },
        { "false", TokenType.FALSE },
        { "fun", TokenType.FUN },
        { "for", TokenType.FOR },
        { "if", TokenType.IF },
        { "nil", TokenType.NIL },
        { "or", TokenType.OR },
        { "print", TokenType.PRINT },
        { "return", TokenType.RETURN },
        { "super", TokenType.SUPER },
        { "this", TokenType.THIS },
        { "true", TokenType.TRUE },
        { "var", TokenType.VAR },
        { "while", TokenType.WHILE },
    };

    internal Scanner(string source)
    {
        this.source = source;
        this.tokens = new List<Token>();
    }

    internal IEnumerable<Token> scanTokens()
    {
        while (!isAtEnd())
        {
            start = current;
            scanToken();
        }

        tokens.Add(new(TokenType.EOF, "", null, line));
        return tokens;
    }

    private void scanToken()
    {
        char c = advance();
        switch (c)
        {
            case '(':
                addToken(TokenType.LEFT_PAREN);
                break;

            case ')':
                addToken(TokenType.RIGHT_PAREN);
                break;

            case '{':
                addToken(TokenType.LEFT_BRACE);
                break;

            case '}':
                addToken(TokenType.RIGHT_BRACE);
                break;

            case ',':
                addToken(TokenType.COMMA);
                break;

            case '.':
                addToken(TokenType.DOT);
                break;

            case '-':
                addToken(TokenType.MINUS);
                break;

            case '+':
                addToken(TokenType.PLUS);
                break;

            case ';':
                addToken(TokenType.SEMICOLON);
                break;

            case '*':
                addToken(TokenType.STAR);
                break;

            case '!':
                addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;

            case '=':
                addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;

            case '<':
                addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;

            case '>':
                addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;

            case '/':
                if (match('/'))
                {
                    while (peek() != '\n' && !isAtEnd())
                        advance();
                }
                else
                {
                    addToken(TokenType.SLASH);
                }
                break;

            case ' ':
            case '\r':
            case '\t':
                // ignore whitespace
                break;

            case '"':
                String();
                break;

            case '\n':
                line++;
                break;

            case 'o':
                if (match('r'))
                    addToken(TokenType.OR);
                break;

            default:
                if (isDigit(c))
                {
                    Number();
                    break;
                }
                else if (isAlpha(c))
                {
                    identifier();
                    break;
                }

                Lox.Error(line, "Unexpected Camera.");
                break;

        }
    }

    private bool isAlpha(char c)
        => (c >= 'a' && c <= 'z') ||
           (c >= 'A' && c <= 'Z') ||
           c == '_';

    private bool isAlphaNumeric(char c)
        => isAlpha(c) || isDigit(c);

    private void identifier()
    {
        while (isAlphaNumeric(peek()))
            advance();

        string text = source.Substring(start, current - start);
        if (!keywords.TryGetValue(text, out TokenType tokenType))
            tokenType = TokenType.IDENTIFIER;

        addToken(tokenType);
    }

    private void Number()
    {
        while (isDigit(peek()))
            advance();

        // Look for a fractional part
        if (peek() == '.' && isDigit(peekNext()))
        {
            // Consume the '.'
            advance();

            while (isDigit(peek()))
                advance();
        }

        string number = source.Substring(start, current - start);
        double value = double.Parse(number);

        addToken(TokenType.NUMBER, value);
    }

    private char peekNext()
    {
        if (current + 1 >= source.Length)
            return '\0';

        return source[current + 1];
    }

    private bool isDigit(char c) => c >= '0' && c <= '9';

    private void String()
    {
        while (peek() != '"' && !isAtEnd())
        {
            if (peek() == '\n')
                line++;
            advance();
        }

        if (isAtEnd())
        {
            Lox.Error(line, "Unterminated string!");
            return;
        }

        advance(); // The Closing ".

        // Trim the surrounding quotes.
        string value = source.Substring(start + 1, current - start - 1);
        addToken(TokenType.STRING, value);
    }

    private char peek() => isAtEnd() ? '\0' : source[current];

    private bool match(char expected)
    {
        if (isAtEnd())
            return false;
        if (source[current] != expected)
            return false;

        current++;
        return true;
    }

    private void addToken(TokenType tokenType)
        => addToken(tokenType, null);

    private void addToken(TokenType tokenType, object literal)
    {
        string text = source.Substring(start, current - start);
        tokens.Add(new(tokenType, text, literal, line));
    }

    private char advance() => source[current++];

    private bool isAtEnd()
        => current >= source.Length;

}