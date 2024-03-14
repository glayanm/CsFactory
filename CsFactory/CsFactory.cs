using System.Reflection;

namespace CsFactory;

public static class CsFactory
{
    private static readonly Dictionary<Type, List<object>> _cache = new();


    public static void Clear()
    {
        _cache.Clear();
    }

    public static T Query<T>(Func<T, bool>? condition = null) where T : new()
    {
        var objects = _cache.ContainsKey(typeof(T)) ? _cache[typeof(T)] : new List<object>();
        var instance = new T();
        if (condition == null)
        {
            instance = objects.Any() ? (T)objects.First()! : Create<T>();
        }
        else
        {
            if (objects.Any(p => condition((T)p))) instance = (T)objects.FirstOrDefault(p => condition((T)p))!;
            // throw new Exception("query result is null.");
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


    public static T Fork<T>(Func<T, bool>? condition = null) where T : class, new()
    {
        var obj = Query(condition);

        return Cloned<T>(obj) ?? new T();
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

    public static T ToForkExpected<T>(this object obj, Action<T>? action) where T : class
    {
        var cloned = Cloned<T>(obj);

        action?.Invoke(cloned!);

        return cloned!;
    }

    private static T? Cloned<T>(object obj) where T : class
    {
        var type = typeof(T);
        var cloned = Activator.CreateInstance(type) as T;

        foreach (var propertyInfo in type.GetProperties())
        {
            if (!propertyInfo.CanRead || !propertyInfo.CanWrite) continue;

            var value = propertyInfo.GetValue(obj);
            propertyInfo.SetValue(cloned, value);
        }

        foreach (var fieldInfo in type.GetFields())
        {
            var value = fieldInfo.GetValue(obj);
            fieldInfo.SetValue(cloned, value);
        }

        return cloned;
    }
}

public class Spec
{
    public T DeepClone<T>()
    {
        return (T)MemberwiseClone();
    }
}