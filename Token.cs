class Token
{
	readonly TokenType type;
	readonly string lexeme;
	readonly object literal;
	readonly int line;

	internal Token(TokenType type, string lexeme, object literal, int line)
	{
		this.type = type;
		this.lexeme = lexeme;
		this.literal = literal;
		this.line = line;
	}

	public override string ToString() => $"TYPE:{type} LEX:{lexeme} LIT:{literal}";

}