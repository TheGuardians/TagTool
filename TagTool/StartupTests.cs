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
			foreach (var method in typeof(StartupTests).GetRuntimeMethods())
			{
				if (!method.IsPrivate)
					continue;

				method.Invoke(null, null);
			}
		}

		#region Test Methods
		private static void ConfirmTagStructureHasNoFields()
		{
			var fields = typeof(TagStructure).GetFields(BindingFlags.Instance | BindingFlags.Public);

			Test.Assert(fields.Length == 0,
				$"`{typeof(TagStructure).FullName}` must not have public instance fields.");
		}

		private static void ValidateTagStructureTypes()
		{
			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
                if (type.IsAbstract)
                    continue;

				var inheritsTagStructure = type.IsSubclassOf(typeof(TagStructure));
				var inheritsAttribute = type.IsDefined(typeof(TagStructureAttribute));

				if (inheritsTagStructure)
					Test.Assert(inheritsAttribute,
						$"{type.FullName} must define [{typeof(TagStructureAttribute).FullName}] because it inherits {typeof(TagStructure).FullName}.");

				if (inheritsAttribute)
					Test.Assert(inheritsTagStructure,
						$"{type.FullName} must inherit {typeof(TagStructure).FullName} becuse it defnes [{typeof(TagStructureAttribute).FullName}].");
			}
		}
		#endregion

		private static class Test
		{
#if DEBUG
			public static void Assert(bool condition, string message)
			{
				// TODO: Can we make visual studio highlight a different line of code when this exception is thrown?
				if (!condition)
					throw new StartupTestException($"FAIL: {message}");
			}
#else
			public static void Assert(bool condition, string message)
			{
				if (!condition)
					Console.WriteLine($"FAIL: {message}");
			}
#endif
		}

		private class StartupTestException : Exception
		{
			public StartupTestException() { }
			public StartupTestException(string message) : base(message) { }
			public StartupTestException(string message, Exception inner) : base(message, inner) { }
			protected StartupTestException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}
	}
}