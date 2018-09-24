//#define STARTUP_TESTS_ENABLED

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TagTool.Serialization;

namespace TagTool.Commands
{
	public static class StartupTests
	{
		private delegate void TestDelegate(ref bool result);

		public static void Run()
		{
#if DEBUG && STARTUP_TESTS_ENABLED
			foreach (var method in typeof(StartupTests).GetRuntimeMethods())
			{
				if (method.IsDefined(typeof(StartupTestAttribute)))
				{
					var info = method.GetCustomAttribute<StartupTestAttribute>();
					if (!info.Enabled)
						continue;

					bool success = true;
					var color = Console.ForegroundColor;
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.WriteLine(info.Text);
					Console.ForegroundColor = color;
					(method.CreateDelegate(typeof(TestDelegate)) as TestDelegate)(ref success);

					if (success)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("		*** SUCCESS ***");
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("		*** FAILURE ***");
					}

					Console.ForegroundColor = color;
					Console.WriteLine();
				}
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
