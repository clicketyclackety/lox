internal sealed class GenerateAst
{
    public static void main(string[] args)
    {
        if (args.Length != 1)
            throw new IOException("No");

        string outputDir = args[0];
        defineAst(outputDir, "Expr", new List<string>
        {
            "Binary    : Expr left, Token operator, Expr right",
            "Grouping  : Expr expression",
            "Literal   : Object value",
            "Unary     : Token operator, Expr right",
        });
    }

    private static void defineAst(string outputDir, string baseName, IEnumerable<string> types)
    {
        string path = Path.Combine(outputDir, baseName, ".cs");

        List<string> values = new()
        {
            $"public abstract class {baseName} {{",
        };

        foreach (string type in types)
        {
            string className = type.Split(":")[0].Trim();
            string fields = type.Split(":")[1].Trim();
            values.AddRange(defineType(baseName, className, fields));
        }

        values.Add("}");

        File.WriteAllLines(path, values);
    }

    private static IEnumerable<string> defineType(string baseName, string className, string fieldList)
    {
        yield return $"public class {className} : {baseName} {{";
        yield return "\n";

        yield return $" public {className}({fieldList}) {{";
        yield return "\n";

        string[] fields = fieldList.Split(", ");
        foreach (var field in fields)
        {
            string name = field.Split(" ")[1];
            yield return $"      this.{name} = {name};";
        }

        yield return "\n";
        yield return "  }";
        yield return "\n";

        foreach (string field in fields)
        {
            yield return $"     final {field};";
        }

        yield return "}}";
        yield return "\n";
    }
}