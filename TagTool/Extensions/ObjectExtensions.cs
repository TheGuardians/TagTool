using System.Reflection;

namespace System
{
    public static class ObjectExtensions
    {
        public static T DeepClone<T>(this T data)
        {
            if (data == null)
                return data;

            var type = data.GetType();

            if (type.IsPrimitive)
                return data;

            object result = null;

            if (type.IsArray)
            {
                var array = data as Array;
                var arrayType = array.GetType();
                var elementType = arrayType.GetElementType();

                result = (Array)Activator.CreateInstance(arrayType, new object[] { });

                for (var i = 0; i < array.Length; i++)
                    ((Array)result).SetValue(array.GetValue(i).DeepClone(), i);

                return (T)result;
            }

            result = Activator.CreateInstance(type, new object[] { });

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic))
                field.SetValue(result, field.GetValue(data).DeepClone());

            return (T)result;
        }
    }
}