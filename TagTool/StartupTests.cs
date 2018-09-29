using System;
using System.Collections.Generic;
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
		private static void CheckStructureSizes()
		{
			var cacheVersions = Enum.GetValues(typeof(CacheVersion)) as CacheVersion[];

			foreach (var type in typeof(TagStructure).Assembly.GetTypes())
			{
				if (type.IsGenericTypeDefinition)
					continue;

				if (type == typeof(TagDefinition))
					continue;

				var inheritsTagStructure = type.IsSubclassOf(typeof(TagStructure));
				var inheritsAttribute = type.IsDefined(typeof(TagStructureAttribute));

				Test.Assert(inheritsTagStructure == inheritsAttribute,
					$"{type.FullName}: {inheritsTagStructure} != {inheritsAttribute}");

				if (!inheritsTagStructure)
					continue;

				var error = false;
				foreach (var cacheVersion in cacheVersions)
				{
					if (cacheVersion < CacheVersion.Halo3Retail || cacheVersion > CacheVersion.HaloOnline106708)
						continue;

					var attribute = TagStructure.GetTagStructureAttribute(type, cacheVersion);
					if (attribute is null)
						continue;

					var structureInfo = TagStructure.GetTagStructureInfo(type, cacheVersion);
					var tagFields = TagStructure.GetTagFieldEnumerable(structureInfo);

					if (structureInfo.TotalSize == 0)
						continue;

					var size = 0U;

					foreach (var tagField in tagFields)
					{
						if (!CacheVersionDetection.IsBetween(tagField.Attribute.Version, cacheVersion, cacheVersion))
							continue;

						if (tagField.Attribute.Runtime)
							continue;

						if (tagField.Attribute.Length != 0)
						{
							if (tagField.FieldType == typeof(string))
							{
								size += (uint)tagField.Attribute.Length;
								continue;
							}

							var elementType = tagField.FieldType.GetElementType() ?? tagField.FieldType;
							if (elementType is null)
								elementType = tagField.FieldType;
							var elementSize = TagFieldInfo.GetFieldSize(elementType, TagFieldAttribute.Default);
							size += (uint)tagField.Attribute.Length * elementSize;
							continue;
						}

						if (tagField.FieldType.IsGenericType && tagField.FieldType.GetGenericTypeDefinition() == typeof(TagBlock<>))
						{
							if (!CacheVersionDetection.IsBetween(cacheVersion, CacheVersion.Halo2Xbox, CacheVersion.Halo2Vista))
								size += 0xC;
							else
								size += 0x8;
						}
						else if (tagField.FieldType.IsGenericType && tagField.FieldType.GetGenericTypeDefinition() == typeof(List<>))
						{
							if (!CacheVersionDetection.IsBetween(cacheVersion, CacheVersion.Halo2Xbox, CacheVersion.Halo2Vista))
								size += 0xC;
							else
								size += 0x8;
						}
						else if (tagField.FieldType == typeof(CachedTagInstance))
						{
							if (!CacheVersionDetection.IsBetween(cacheVersion, CacheVersion.Halo2Xbox, CacheVersion.Halo2Vista))
								size += 0x10;
							else
								size += 0x8;
						}
						else if (tagField.FieldType == typeof(byte[]))
						{
							if (!CacheVersionDetection.IsBetween(cacheVersion, CacheVersion.Halo2Xbox, CacheVersion.Halo2Vista))
								size += 0x14;
							else
								size += 0x8;
						}
						else
							size += tagField.Size;
					}

					if (structureInfo.TotalSize != size)
					{
						Console.WriteLine($"{cacheVersion} {type.FullName}: 0x{structureInfo.TotalSize.ToString("X4")} != 0x{size.ToString("X4")}");
						error = true;
					}
				}

				if (error)
					Console.WriteLine();
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
			public static void Assert(bool condition, string message = null)
			{
				// TODO: Can we make visual studio highlight a different line of code when this exception is thrown?
				if (!condition)
					throw new StartupTestException($"FAIL: {message}");
			}
#else
			public static void Assert(bool condition, string message = null)
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