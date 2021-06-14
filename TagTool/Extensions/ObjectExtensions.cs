using System.Collections;
using System.Reflection;
using TagTool.Cache;
using TagTool.Common;

namespace System
{
    public static class ObjectExtensions
    {
        // TODO: verify that this can be replaced with DeepCloneV2 without breaking things
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

        public static object DeepCloneV2(this object data)
        {
            if (data == null)
                return null;

            var type = data.GetType();
            if (type.IsValueType)
                return data;

            switch (data)
            {
                case CachedTag tag:
                    return tag;
                case IList list:
                    {
                        if (type.IsArray)
                        {
                            var cloned = (IList)Activator.CreateInstance(type, new object[] { list.Count });
                            for (int i = 0; i < cloned.Count; i++)
                                cloned[i] = list[i].DeepCloneV2();
                            return cloned;
                        }
                        else
                        {
                            var cloned = (IList)Activator.CreateInstance(type);
                            foreach (var element in list)
                                cloned.Add(element.DeepCloneV2());
                            return cloned;
                        }
                    }
                case ICloneable cloneable:
                    return cloneable.Clone();
                default:
                    {
                        var cloned = Activator.CreateInstance(type);
                        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                            field.SetValue(cloned, field.GetValue(data).DeepCloneV2());
                        return cloned;
                    }
            }
        }

        public static T DeepCloneV2<T>(this T data)
        {
            return (T)DeepCloneV2((object)data);
        }

        public static bool IsBlamType(this Type type) => typeof(IBlamType).IsAssignableFrom(type);
    }
}