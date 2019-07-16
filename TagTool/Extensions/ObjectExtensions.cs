using System.Reflection;
using TagTool.Common;

namespace System
{
    public static class ObjectExtensions
    {
        public static T DeepClone<T>(this T data)
        {
            if (data == null)
                return data;

            var type = data.GetType();
            var isString = type == typeof(string);

            if (type.IsPrimitive)
                return data;

            if (isString)
                type = typeof(char[]);

            object result = null;

            if (type.IsArray)
            {
                var array = isString ? (data as string).ToCharArray() : data as Array;
                var arrayType = array.GetType();
                var elementType = arrayType.GetElementType();

                result = (Array)Activator.CreateInstance(arrayType, new object[] {array.Length});

                for (var i = 0; i < array.Length; i++)
                    ((Array)result).SetValue(array.GetValue(i).DeepClone(), i);

                return isString ? (T)(object)new string((char[])result) : (T)result;
            }

            result = Activator.CreateInstance(type, new object[] { });

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                field.SetValue(result, field.GetValue(data).DeepClone());

            return (T)result;
        }

        public static bool IsBlamType(this Type type) => typeof(IBlamType).IsAssignableFrom(type);
    }
}