public interface Expr<T>
{
    public T accept(Visitor<T> visitor);
}
public interface Visitor<T>
{
    public T visitBinaryExpr(Binary expr);
    public T visitGroupingExpr(Grouping expr);
    public T visitLiteralExpr(Literal expr);
    public T visitUnaryExpr(Unary expr);
}

public class Binary : Expr<Binary>
{
    internal readonly Expr<Binary> left;
    internal readonly Token op;
    internal readonly Expr<Binary> right;

    public Binary(Expr<Binary> left, Token op, Expr<Binary> right)
    {
        this.left = left;
        this.op = op;
        this.right = right;
    }

    public Binary accept(Visitor<Binary> visitor)
    {
        return visitor.visitBinaryExpr(this);
    }

}

public class Grouping : Expr<Grouping>
{
    internal readonly Expr<Grouping> expression;

    public Grouping(Expr<Grouping> expression)
    {
        this.expression = expression;
    }

    public Grouping accept(Visitor<Grouping> visitor)
    {
        return visitor.visitGroupingExpr(this);
    }

}

public class Literal : Expr<Literal>
{
    internal readonly object value;

    public Literal(object value)
    {
        this.value = value;
    }

    public Literal accept(Visitor<Literal> visitor)
    {
        return visitor.visitLiteralExpr(this);
    }

}

public class Unary : Expr<Unary>
{
    internal readonly Token op;
    internal readonly Expr<Unary> right;

    public Unary(Token op, Expr<Unary> right)
    {
        this.op = op;
        this.right = right;
    }

    public Unary accept(Visitor<Unary> visitor)
    {
        return visitor.visitUnaryExpr(this);
    }

}

