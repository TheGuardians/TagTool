using System;
using System.Diagnostics;
using System.Reflection;
using TagTool.Tags;

namespace TagTool.Commands
{
    public static class StartupTests
	{
		public static void Run()
		{
#if DEBUG
			foreach (var method in typeof(StartupTests).GetRuntimeMethods())
			{
				var info = method.GetCustomAttribute<StartupTestAttribute>();
				if (info is null || !info.Enabled)
					continue;

				// Run the test
				Console.WriteLine(info.Text);
				method.Invoke(null, null);
			}

			Console.WriteLine();
#endif
		}

		[StartupTest("Verifying `" + nameof(TagStructure) + "` contains no public instance fields...")]
		private static void ConfirmTagStructureHasNoFields()
		{
			var fields = typeof(TagStructure).GetFields(BindingFlags.Instance | BindingFlags.Public);

			Debug.Assert(fields.Length == 0, 
				$"`{nameof(TagStructure)}` must not have public instance fields.");
		}

		[StartupTest("Verifying `" + nameof(TagStructure) + "` inheritance and `[" + nameof(TagStructureAttribute) + "]` usage...")]
		private static void ValidateTagStructureTypes()
		{
			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
				var inheritsTagStructure = type.IsSubclassOf(typeof(TagStructure));
				var inheritsAttribute = type.IsDefined(typeof(TagStructureAttribute));

				if (inheritsTagStructure)
					Debug.Assert(inheritsAttribute,
						$"{type.FullName} must define [{nameof(TagStructureAttribute)}] because it inherits {nameof(TagStructure)}.");

				if (inheritsAttribute)
					Debug.Assert(inheritsTagStructure,
						$"{type.FullName} must inherit {nameof(TagStructure)} becuse it defnes [{nameof(TagStructureAttribute)}].");
			}
		}

		[AttributeUsage(AttributeTargets.Method)]
		private class StartupTestAttribute : Attribute
		{
			public string Text;
			public bool Enabled;
			public StartupTestAttribute(string text, bool enabled = true)
			{
				Text = text;
				Enabled = enabled;
			}
		}
	}
}
