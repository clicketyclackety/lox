using lox.tools;

namespace lox_tests
{

	public class Tests
	{

		[Test]
		public void Test1()
		{
			var left = new Unary(new Token(TokenType.MINUS, "-", null, 1),
								new Literal(123));

			var token = new Token(TokenType.STAR, "*", null, 1);

			var right = new Grouping(new Literal(45.67));

			var expression = new Binary(left, token, right);

			new AstPrinter().print(expression);
		}
	}

}