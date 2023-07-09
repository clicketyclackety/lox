namespace lox_tests
{

	public class AstPrinterTests
	{

		[Theory]
		[TestCaseSource(nameof(TestData))]
		public void PrinterResult_Is_Correct(string expected, Expr expression)
		{
			string result = new AstPrinter().print(expression);

			Assert.Equals(expected, result);
		}

		public static IEnumerable TestData
		{
			get
			{
				var left = new Unary(new Token(TokenType.MINUS, "-", null, 1),
									new Literal(123));

				var token = new Token(TokenType.STAR, "*", null, 1);

				var right = new Grouping(new Literal(45.67));

				var expression = new Binary(left, token, right);

				string given = "(* (- 123) (group 45.67))";
				yield return new TestCaseData(given, expression);
			}
		}
	}

}