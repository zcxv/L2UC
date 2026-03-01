using System;
using System.Linq;

public static class TypeUtils {
    public static string GetFriendlyName(this Type type) {
        if (type.IsGenericType) {
            string baseName = type.Name.Split('`')[0];
            string genericArgs = string.Join(", ", type.GetGenericArguments().Select(t => t.GetFriendlyName()));
            return $"{baseName}<{genericArgs}>";
        }

        return type.Name;
    }
}