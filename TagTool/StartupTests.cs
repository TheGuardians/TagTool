using System;
using System.Diagnostics;
using System.Reflection;
using TagTool.Serialization;

namespace TagTool.Commands
{
	public static class StartupTests
	{
		private delegate void TestDelegate(ref bool result);

		public static void Run()
		{
#if DEBUG
			foreach (var method in typeof(StartupTests).GetRuntimeMethods())
			{
				var info = method.GetCustomAttribute<StartupTestAttribute>();
				if (info is null || !info.Enabled)
					continue;

				// Print the test startup info.
				var color = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.WriteLine(info.Text);
				Console.ForegroundColor = color;

				// Start a timer and run the test.
				bool success = true;
				var testDelegate = method.CreateDelegate(typeof(TestDelegate)) as TestDelegate;
				var sw = Stopwatch.StartNew();
				testDelegate(ref success);

				// Print the time the test took.
				var milliseconds = sw.ElapsedMilliseconds;
				var output = milliseconds.FormatMilliseconds();
				var startColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Console.WriteLine(output);

				// Print test results.
				if (success)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("		*** SUCCESS ***");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Debug.WriteLine("		*** FAILURE ***");
				}

				// Set color back and write an empty line.
				Console.ForegroundColor = color;
				Console.WriteLine();
			}
#endif
		}

		[StartupTest("Checking `" + nameof(TagStructure) + "` contains no public instance fields.")]
		private static void ConfirmTagStructureHasNoFields(ref bool result)
		{
			var fields = typeof(TagStructure).GetFields(BindingFlags.Instance | BindingFlags.Public);
			AssertLine(fields.Length == 0, $"`{nameof(TagStructure)}` must not have public instance fields.", ref result);
		}

		[StartupTest("Checking all types inheriting `" + nameof(TagStructure) + "` also define `[" + nameof(TagStructureAttribute) + "]`.")]
		private static void ValidateTagStructureTypes(ref bool result)
		{
			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
				var t = type.IsSubclassOf(typeof(TagStructure));
				var a = type.IsDefined(typeof(TagStructureAttribute));

				var fName = type.FullName.Replace("TagTool.Tags.", "").Replace("TagTool.", "");
				AssertLine(t == a, $"TagStructure: {t} | Attribute: {a} | {fName}", ref result);
			}
		}

		private static void AssertLine(bool cond, string error, ref bool result)
		{
			var color = Console.ForegroundColor;
			if (!cond)
			{
				result = false;
				if (!string.IsNullOrWhiteSpace(error))
				{
					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.Write($"ERROR: ");
					Console.ForegroundColor = color;
					Console.WriteLine(error);
				}
			}
		}

		[AttributeUsage(AttributeTargets.Method)]
		private class StartupTestAttribute : Attribute
		{
			public string Text;
			public bool Enabled;
			public StartupTestAttribute(string text, bool enabled = true)
			{
				this.Text = text;
				this.Enabled = enabled;
			}
		}
	}
}
