using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

class Program {
	static void Help() {
		var name = typeof(Program).Assembly.GetName().Name;
		Console.WriteLine($"{name} [options] file...");
		Console.WriteLine("");
		Console.WriteLine("-h  Show help");
		Console.WriteLine("-V  Show version");
	}

	static void Indent(int n) {
		while (0 != n--)
			Console.Write("  ");
	}

	static void Main(string[] args) {
		try {
			var files = new List<string>();
			foreach (var arg in args) {
				var s = arg;
				if (!s.StartsWith('-')) {
					files.Add(s);
					continue;
				}
				while (s.StartsWith('-'))
					s = s[1..];
				switch (s) {
				case "?":
				case "h":
				case "help":
					Help();
					return;
				case "V":
				case "v":
				case "version":
					Version();
					return;
				default:
					throw new Error(arg + ": unknown option");
				}
			}
			foreach (var file in files) {
				var text = File.ReadAllText(file);
				var tree = CSharpSyntaxTree.ParseText(text, CSharpParseOptions.Default, file);
				if (tree.GetDiagnostics().Any()) {
					foreach (var diagnostic in tree.GetDiagnostics())
						Console.Error.WriteLine(diagnostic);
					Environment.Exit(1);
				}
				var root = tree.GetCompilationUnitRoot();
				Print(root);
			}
		} catch (Error e) {
			Console.Error.WriteLine(e.Message);
			Environment.Exit(1);
		}
	}

	static void Print(SyntaxNode node, int level = 0) {
		Indent(level);
		Console.WriteLine(node.Kind());
		foreach (var a in node.ChildNodes())
			Print(a, level + 1);
	}

	static void Version() {
		var name = typeof(Program).Assembly.GetName().Name;
		var version = typeof(Program).Assembly.GetName()?.Version?.ToString(2);
		Console.WriteLine($"{name} {version}");
	}
}
