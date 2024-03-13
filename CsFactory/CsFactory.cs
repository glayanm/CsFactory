using System.Reflection;

namespace CsFactory;

public static class CsFactory
{
    private static readonly Dictionary<Type, List<object>> _cache = new();

    public static T Query<T>(Func<T, bool>? condition = null) where T : new()
    {
        var objects = _cache.ContainsKey(typeof(T)) ? _cache[typeof(T)] : new List<object>();
        var instance = new T();
        if (condition == null)
        {
            instance = (T)objects.FirstOrDefault()! ?? new T();
        }
        else
        {
            if (objects.Any(p => condition((T)p))) instance = (T)objects.FirstOrDefault(p => condition((T)p))!;
        }

        return instance;
    }

    public static T Create<T>(Action<T>? setValue = null) where T : new()
    {
        var objects = _cache.ContainsKey(typeof(T)) ? _cache[typeof(T)] : new List<object>();

        var instance = new T();
        foreach (var property in typeof(T).GetProperties())
        {
            var defaultValue = GetDefaultValue(property, objects.Count);
            property.SetValue(instance, defaultValue);
        }

        setValue?.Invoke(instance);

        objects.Add(instance);

        if (objects.Count == 1) _cache.Add(typeof(T), objects);
        else
            _cache[typeof(T)] = objects;

        return instance;
    }

    public static T Map<T>(this T obj, Action<T> SetValue) where T : class
    {
        SetValue.Invoke(obj);
        return obj;
    }

    private static object? GetDefaultValue(PropertyInfo propertyInfo, int objectsCount)
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

        if (type.IsClass)
        {
            var count = _cache[type].Count;
            if (count != 0)
            {
                var index = objectsCount % count;
                return _cache[type].ToArray()[index];
            }

            return Activator.CreateInstance(type);
        }

        return Activator.CreateInstance(type);
    }
}