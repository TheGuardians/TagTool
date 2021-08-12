using System.Collections.Generic;
using System.Linq;

namespace System
{
	static class EnumExtensions
	{
		public static IEnumerable<Enum> GetFlags(this Enum value)
		{
			return GetFlags(value, Enum.GetValues(value.GetType()).Cast<Enum>().ToArray());
		}

		public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
		{
			return GetFlags(value, GetFlagValues(value.GetType()).ToArray());
		}

		private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
		{
			ulong bits = Convert.ToUInt64(value);
			List<Enum> results = new List<Enum>();
			for (int i = values.Length - 1; i >= 0; i--)
			{
				ulong mask = Convert.ToUInt64(values[i]);
				if (i == 0 && mask == 0L)
					break;
				if ((bits & mask) == mask)
				{
					results.Add(values[i]);
					bits -= mask;
				}
			}
			if (bits != 0L)
				return Enumerable.Empty<Enum>();
			if (Convert.ToUInt64(value) != 0L)
				return results.Reverse<Enum>();
			if (bits == Convert.ToUInt64(value) && values.Length > 0 && Convert.ToUInt64(values[0]) == 0L)
				return values.Take(1);
			return Enumerable.Empty<Enum>();
		}

		private static IEnumerable<Enum> GetFlagValues(Type enumType)
		{
			ulong flag = 0x1;
			foreach (var value in Enum.GetValues(enumType).Cast<Enum>())
			{
				ulong bits = Convert.ToUInt64(value);
				if (bits == 0L)
					//yield return value;
					continue; // skip the zero value
				while (flag < bits) flag <<= 1;
				if (flag == bits)
					yield return value;
			}
		}

        public static bool HasFlag<T>(this T value, T flags) where T: Enum
        {
            return value.HasFlag(flags);
        }

		public static object ConvertLexical(this Enum value, Type targetType)
		{
			var members = value.ToString()
				.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
				.Where(x => targetType.IsEnumDefined(x))
				.ToArray();

			if(members.Length == 0)
				return Activator.CreateInstance(targetType);

			return Enum.Parse(targetType, string.Join(", ", members));
		}

        public static U ConvertLexical<U>(this Enum value) where U : Enum
		{
			return (U)ConvertLexical(value, typeof(U));
		}
	}
}
