namespace GraphOnSharp.Helper
{
    using System.Collections.Generic;
    using System.Reflection;

    class ClassHelper
    {
        public static IEnumerable<FieldInfo> GetFields(object obj)
        {
            var type = obj.GetType();
            return type.GetFields();
        }

        public static bool HasValue(object obj, FieldInfo field)
        {
            var value = GetValue(obj, field);
            return value != null;
        }

        public static object GetValue(object obj, FieldInfo field)
        {
            var value = field.GetValue(obj);
            return value;
        }


        public static bool IsValueType<TValue>(object obj, FieldInfo field, out TValue value)
            where TValue : class
        {
            var fieldValue = field.GetValue(obj);
            value = fieldValue as TValue;
            return value != null;
        }

        public static TValueType GetValue<TValueType>(object obj, FieldInfo field)
            where TValueType : class, new()
        {
            var fieldValue = field.GetValue(obj);
            var value = fieldValue as TValueType;
            if(value is null)
            {
                value = new TValueType();
            }
            return value;
        }

        public static void SetValue(object obj, FieldInfo field, object value)
        {
            if (value is null) { return; }

            field.SetValue(obj, value);
        }
    }
}
