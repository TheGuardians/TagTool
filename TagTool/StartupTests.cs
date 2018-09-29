using System;
using System.Diagnostics;
using System.Reflection;
using TagTool.Cache;
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
        private static void TagStructureSizeTest()
        {
            var cacheVersions = Enum.GetValues(typeof(CacheVersion)) as CacheVersion[];

            foreach (var type in typeof(TagStructure).Assembly.GetTypes())
            {
                foreach (var cacheVersion in cacheVersions)
                {
                    var attribute = TagStructure.GetTagStructureAttribute(type, cacheVersion);
                    Test.Assert(attribute != null,
                        $"{type.FullName}, {type.Name}, {type}, {type.MemberType}, {type.DeclaringType}, " +
                        $"{type.ReflectedType}, {type.IsGenericTypeDefinition}, {type.Namespace}, " +
                        $"{type.AssemblyQualifiedName}, {type.GetMembers().Length}");

                    var structureInfo = TagStructure.GetTagStructureInfo(type, cacheVersion);
                    var tagFields = TagStructure.GetTagFieldEnumerable(structureInfo);

                    var size = 0U;
                    foreach (var tagField in tagFields)
                    {
                        size += tagField.Size;
                        Test.Assert(size > 0, "");
                    }

                    Test.Assert(structureInfo.TotalSize == size, "");
                }
            }
        }

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