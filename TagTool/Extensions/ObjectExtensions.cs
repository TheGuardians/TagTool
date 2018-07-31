using System.Reflection;

namespace System
{
    public static class ObjectExtensions
    {
        public static object DeepClone(this object data)
        {
            if (data == null)
                return null;

            var type = data.GetType();

            if (type.IsPrimitive)
                return data;

            object result = null;

            if (type.IsArray)
            {
                var array = (Array)data;
                var arrayType = array.GetType();
                var elementType = arrayType.GetElementType();

                result = (Array)Activator.CreateInstance(arrayType, new object[] { });

                for (var i = 0; i < array.Length; i++)
                    ((Array)result).SetValue(array.GetValue(i).DeepClone(), i);

                return array;
            }

            result = Activator.CreateInstance(type, new object[] { });

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic))
                field.SetValue(result, field.GetValue(data).DeepClone());

            return result;
        }
    }
}