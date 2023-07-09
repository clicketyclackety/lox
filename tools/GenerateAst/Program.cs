﻿public class GenerateAst
{
    static void Main(params string[] args)
    {
        if (args.Length != 1)
            throw new IOException("No");

        string outputDir = args[0];
        defineAst(outputDir, "Expr", new List<string>
        {
            "Binary    : Expr<Binary> left, Token op, Expr<Binary> right",
            "Grouping  : Expr<Grouping> expression",
            "Literal   : object value",
            "Unary     : Token op, Expr<Unary> right",
        });
    }

    private static void defineAst(string outputDir, string baseName, IEnumerable<string> types)
    {
        string path = Path.Combine(outputDir, $"{baseName}.cs");

        List<string> values = new()
        {
            $"public interface {baseName}<T>\n{{",
            "    public T accept(Visitor<T> visitor);",
            "}",
        };

        values.AddRange(defineVisitor(baseName, types));     

        foreach (string type in types)
        {
            string className = type.Split(":")[0].Trim();
            string fields = type.Split(":")[1].Trim();
            values.AddRange(defineType(baseName, className, fields));
        }

        File.WriteAllLines(path, values);
    }

    private static IEnumerable<string> defineVisitor(string baseName, IEnumerable<string> types)
    {
        yield return "public interface Visitor<T>\n{";
        
        foreach(string type in types)
        {
            string typeName = type.Split(":")[0].Trim();
            yield return $"    public T visit{typeName}{baseName}({typeName} {baseName.ToLower()});";
        }
        yield return "}\n";
    }

    private static IEnumerable<string> defineType(string baseName, string className, string fieldList)
    {
        yield return $"public class {className} : {baseName}<{className}>\n{{";

        string[] fields = fieldList.Split(", ");
        
        foreach (string field in fields)
        {
            yield return $"    internal readonly {field};";
        }

        yield return $"\n    public {className}({fieldList})\n    {{";
        foreach (var field in fields)
        {
            string name = field.Split(" ")[1];
            yield return $"        this.{name} = {name};";
        }
        yield return "    }\n";

        yield return $"    public {className} accept(Visitor<{className}> visitor)";
        yield return "    {";
        yield return $"        return visitor.visit{className}{baseName}(this);";
        yield return "    }";

        yield return "\n}\n";
    }
}