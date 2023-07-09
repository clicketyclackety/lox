public interface Expr
{
    public V accept<V>(IVisitor<V> visitor);
}
public interface IVisitor<T>
{
    public T visitBinaryExpr(Binary expr);
    public T visitGroupingExpr(Grouping expr);
    public T visitLiteralExpr(Literal expr);
    public T visitUnaryExpr(Unary expr);
}

public class Binary : Expr
{
    internal readonly Expr left;
    internal readonly Token op;
    internal readonly Expr right;

    public Binary(Expr left, Token op, Expr right)
    {
        this.left = left;
        this.op = op;
        this.right = right;
    }

    public Binary accept<Binary>(IVisitor<Binary> visitor)
    {
        return visitor.visitBinaryExpr(this);
    }

}

public class Grouping : Expr
{
    internal readonly Expr expression;

    public Grouping(Expr expression)
    {
        this.expression = expression;
    }

    public Grouping accept<Grouping>(IVisitor<Grouping> visitor)
    {
        return visitor.visitGroupingExpr(this);
    }

}

public class Literal : Expr
{
    internal readonly object value;

    public Literal(object value)
    {
        this.value = value;
    }

    public Literal accept<Literal>(IVisitor<Literal> visitor)
    {
        return visitor.visitLiteralExpr(this);
    }

}

public class Unary : Expr
{
    internal readonly Token op;
    internal readonly Expr right;

    public Unary(Token op, Expr right)
    {
        this.op = op;
        this.right = right;
    }

    public Unary accept<Unary>(IVisitor<Unary> visitor)
    {
        return visitor.visitUnaryExpr(this);
    }

}

