using System.Reflection;

namespace CsFactory;

public class Class1
{
    public class CsFactory
    {
        public T Create<T>() where T : new()
        {
            var instance = new T();

            foreach (var property in typeof(T).GetProperties())
            {
                var defaultValue = GetDefaultValue(property);
                property.SetValue(instance, defaultValue);
            }

            return instance;
        }

        private object? GetDefaultValue(PropertyInfo propertyInfo)
        {
            var number = 0;
            var type = propertyInfo.PropertyType;

            if (type == typeof(int))
            {
                return number;
            }

            if (type == typeof(string))
            {
                return propertyInfo.Name + "#number";
            }

            return Activator.CreateInstance(type);
        }
    }
}