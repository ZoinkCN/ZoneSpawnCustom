using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ZoneSpawnCustom.Classes
{
    public class WritableList<T> : List<T>, IJsonWritable
    {
        public void Write(IJsonWriter writer)
        {
            Type innerType = typeof(T);
            var size = Count;
            var innerValueProperties = innerType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            writer.ArrayBegin(size);
            foreach (var value in this)
            {
                switch (value)
                {
                    case string stringValue:
                        writer.Write(stringValue);
                        break;
                    case float singleValue:
                        writer.Write(singleValue);
                        break;
                    case double doubleValue:
                        writer.Write(doubleValue);
                        break;
                    case short int16Value:
                        writer.Write(int16Value);
                        break;
                    case uint uint32Value:
                        writer.Write(uint32Value);
                        break;
                    case int int32Value:
                        writer.Write(int32Value);
                        break;
                    case long int64Value:
                        writer.Write(int64Value);
                        break;
                    case bool boolValue:
                        writer.Write(boolValue);
                        break;
                    case decimal decimalValue:
                        writer.Write((int)(decimalValue * 100)); // Writing decimal as INT up two decimal places
                        break;
                    case Enum enumValue:
                        writer.Write(enumValue.ToString()); // Writing enum as string
                        break;
                    default:
                        throw new Exception($"Type {value.GetType()} is not supported");
                }
            }
            writer.ArrayEnd();
        }
    }

    public static class Extends
    {
        public static WritableList<TSource> ToWritableList<TSource>(this IEnumerable<TSource> source)
        {
            var result = new WritableList<TSource>();
            result.AddRange(source);
            return result;
        }
    }
}
