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
				if (!method.IsPrivate)
					continue;

				method.Invoke(null, null);
			}

			Console.WriteLine();
#endif
		}

		private const string IgnoreStackMsg = "\n*** Ignore The Stacktrace Below ***\n";

		private static void ConfirmTagStructureHasNoFields()
		{
			Console.WriteLine(
				$"Verifying `{typeof(TagStructure).FullName}` contains no public instance fields...");

			var fields = typeof(TagStructure).GetFields(BindingFlags.Instance | BindingFlags.Public);

			Debug.Assert(fields.Length == 0,
				$"`{typeof(TagStructure).FullName}` must not have public instance fields.", IgnoreStackMsg);
		}

		private static void ValidateTagStructureTypes()
		{
			Console.WriteLine(
				$"Verifying `{typeof(TagStructure).FullName}` inheritance and `[{typeof(TagStructureAttribute).FullName}]` usage...");

			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
				var inheritsTagStructure = type.IsSubclassOf(typeof(TagStructure));
				var inheritsAttribute = type.IsDefined(typeof(TagStructureAttribute));

				if (inheritsTagStructure)
					Debug.Assert(inheritsAttribute,
						$"{type.FullName} must define [{typeof(TagStructureAttribute).FullName}] because it inherits {typeof(TagStructure).FullName}.", IgnoreStackMsg);

				if (inheritsAttribute)
					Debug.Assert(inheritsTagStructure,
						$"{type.FullName} must inherit {typeof(TagStructure).FullName} becuse it defnes [{typeof(TagStructureAttribute).FullName}].", IgnoreStackMsg);
			}
		}
	}
}
