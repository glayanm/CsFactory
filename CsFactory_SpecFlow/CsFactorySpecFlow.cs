using System.Reflection;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace CsFactory_SpecFlow;


public static class CsFactorySpecFlow
{
    public static void ShouldMatch<T>(this Table table, List<T> actualList) where T : class, new()
    {
        var i = 0;
        foreach (var tableRow in table.Rows)
        {
            var actual = actualList[i];
            foreach (var property in typeof(T).GetProperties())
                if (tableRow.Keys.Any(p => p == property.Name))
                {
                    var setValue = tableRow.FirstOrDefault(p =>
                        p.Key == property.Name).Value;

                    var propertyType = property.PropertyType;
                    var value = propertyType == typeof(int) ? int.Parse(setValue) :
                        propertyType == typeof(string) ? setValue :
                        propertyType == typeof(decimal) ? decimal.Parse(setValue) :
                        propertyType == typeof(long) ? long.Parse(setValue) :
                        propertyType == typeof(bool) ? bool.Parse(setValue) :
                        propertyType == typeof(DateTime) ? DateTime.Parse(setValue) :
                        propertyType.IsEnum ? Enum.Parse(propertyType, setValue) :
                        Activator.CreateInstance(propertyType);

                    var assertStr = GetTableAssertMessage(property, actual, tableRow);
                    Assert.AreEqual(value, property.GetValue(actual), assertStr);
                }
        }
    }

    private static string? GetTableAssertMessage<T>(PropertyInfo property, T actual, TableRow table)
        where T : class, new()
    {
        var result = new StringBuilder();
        var actualString = new StringBuilder();
        var errorString = new StringBuilder();
        result.Append($"Expected data is : \r\n ");

        foreach (var propertyInfo in typeof(T).GetProperties())
        {
            if (table.Keys.Any(p => p == propertyInfo.Name))
            {
                var expectedValue = table.First(p => p.Key == propertyInfo.Name).Value;
                var actualValue = propertyInfo.GetValue(actual);
                var maxLen = Math.Max(3, Math.Max(expectedValue.Length, actualValue.ToString().Length));
                maxLen = maxLen < 3 ? 3 : maxLen;
                result.Append($" | {expectedValue.PadRight(maxLen, ' ')}");
                actualString.Append($" | {actualValue.ToString().PadRight(maxLen, ' ')}");
                errorString.Append(property.Name == propertyInfo.Name
                    ? "   ↑↑↑".PadRight(maxLen - 3, ' ')
                    : "".PadRight(maxLen + 3, ' '));
            }
        }

        result.Append($" | \r\n ");
        result.Append(errorString + "\r\n");
        actualString.Append($" | \r\n ");
        result.Append($"Actual data is : \r\n ");
        result.Append(actualString);
        result.Append(errorString);
        return result.ToString();
    }


    public static List<T> CreateToList<T>(this Table table) where T : class, new()
    {
        var result = new List<T>();
        foreach (var tableRow in table.Rows)
        {
            var instance = CsFactory.CsFactory.Create<T>();
            foreach (var property in typeof(T).GetProperties())
                if (tableRow.Keys.Any(p => p == property.Name))
                {
                    var setValue = tableRow.FirstOrDefault(p =>
                        p.Key == property.Name).Value;

                    var propertyType = property.PropertyType;
                    var value = propertyType == typeof(int) ? int.Parse(setValue) :
                        propertyType == typeof(string) ? setValue :
                        propertyType == typeof(decimal) ? decimal.Parse(setValue) :
                        propertyType == typeof(long) ? long.Parse(setValue) :
                        propertyType == typeof(bool) ? bool.Parse(setValue) :
                        propertyType == typeof(DateTime) ? DateTime.Parse(setValue) :
                        propertyType.IsEnum ? Enum.Parse(propertyType, setValue) :
                        Activator.CreateInstance(propertyType);
                    property.SetValue(instance, value);
                }

            result.Add(instance);
        }

        return result;
    }
}