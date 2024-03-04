using System.Reflection;

namespace CsFactory;

public class CsFactory
{
    private readonly Dictionary<Type, List<object>> _cache = new();


    public T Create<T>() where T : new()
    {
        var objects = _cache.ContainsKey(typeof(T)) ? _cache[typeof(T)] : new List<object>();
        var instance = new T();

        foreach (var property in typeof(T).GetProperties())
        {
            var defaultValue = GetDefaultValue(property, objects.Count);
            property.SetValue(instance, defaultValue);
        }

        objects.Add(instance);

        if (objects.Count == 1) _cache.Add(typeof(T), objects);
        else
            _cache[typeof(T)] = objects;

        return instance;
    }

    private object? GetDefaultValue(PropertyInfo propertyInfo, int objectsCount)
    {
        var number = objectsCount + 1;
        var type = propertyInfo.PropertyType;

        if (type == typeof(int)) return number;
        if (type == typeof(decimal)) return (decimal)number;
        if (type == typeof(long)) return (long)number;
        if (type == typeof(bool)) return false;

        if (type.IsEnum)
        {
            var enumValues = Enum.GetValues(type);
            return enumValues.Length > 0 ? enumValues.GetValue(0) : null;
        }

        if (type == typeof(DateTime)) return DateTime.Today.AddMinutes(number);
        if (type == typeof(string)) return $"{propertyInfo.Name}#{number}";

        return Activator.CreateInstance(type);
    }
}