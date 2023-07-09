using System.Text;

namespace lox
{

	public sealed class AstPrinter : IVisitor<string>
	{
		public string print(Expr expr)
			=> expr.accept(this);

		public string visitBinaryExpr(Binary expr)
			=> parenthize(expr.op.lexeme, expr.left, expr.right);

		public string visitGroupingExpr(Grouping expr)
			=> parenthize("group", expr.expression);

		public string visitLiteralExpr(Literal expr)
		{
			if (expr.value is null) return "nil";
			return expr.value.ToString();
		}

		public string visitUnaryExpr(Unary expr)
			=> parenthize(expr.op.lexeme, expr.right);

		private string parenthize(string name, params Expr[] exprs)
		{
			StringBuilder builder = new();
			builder.Append('(').Append(name);
			foreach (Expr expr in exprs)
			{
				builder.Append(' ');
				builder.Append(expr.accept(this));
			}
			builder.Append(')');

			return builder.ToString();
		}

	}

}
