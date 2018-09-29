using Microsoft.VisualStudio.TestTools.UnitTesting;
using TagTool.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using System.Reflection;
using System.Diagnostics;

namespace TagTool.Tags.Tests
{
	[TestClass()]
	public class TagStructureTests
	{
		[TestMethod()]
		public void TestGen3StructureSizes()
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

				Assert.IsTrue(inheritsTagStructure == inheritsAttribute,
					$"{type.FullName}: {inheritsTagStructure} != {inheritsAttribute}");

				if (!inheritsTagStructure)
					continue;

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

					Assert.IsTrue(structureInfo.TotalSize != size,
						$"{cacheVersion} {type.FullName}: 0x{structureInfo.TotalSize.ToString("X4")} != 0x{size.ToString("X4")}");
				}
			}
		}
	}
}