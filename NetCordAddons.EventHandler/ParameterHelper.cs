using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NetCordAddons.EventHandler;

internal static class ParameterHelper
{
    public static object? GetParameterDefaultValue(Type type, ParameterInfo parameter)
    {
        var underlyingType = Nullable.GetUnderlyingType(type);
        return underlyingType is null
            ? GetNonUnderlyingTypeDefaultValue(type, parameter)
            : GetUnderlyingTypeDefaultValue(underlyingType, parameter);
    }

    public static object? GetNonUnderlyingTypeDefaultValue(Type type, ParameterInfo parameter)
    {
        if (!type.IsValueType) return parameter.DefaultValue;

        if (type.IsPrimitive)
        {
            var defaultValue = parameter.DefaultValue;
            if (type == typeof(nint))
                return (nint)(int)defaultValue!;
            if (type == typeof(nuint))
                return (nuint)(uint)defaultValue!;
            return defaultValue;
        }

        if (type == typeof(decimal) || type.IsEnum)
            return parameter.DefaultValue;
        return GetDefaultValue(type);
    }

    public static object? GetUnderlyingTypeDefaultValue(Type type, ParameterInfo parameter)
    {
        if (type.IsPrimitive)
        {
            var defaultValue = parameter.DefaultValue;
            if (type == typeof(nint))
                return defaultValue is null ? null : (nint)(int)defaultValue;
            if (type == typeof(nuint))
                return defaultValue is null ? null : (nuint)(uint)defaultValue;
            return defaultValue;
        }

        if (type == typeof(decimal))
            return parameter.DefaultValue;
        if (type.IsEnum)
            return GetUnderlyingEnumDefaultValue(type, parameter);
        return null;
    }

    public static Expression GetParameterDefaultValueExpression(Type type, ParameterInfo parameter)
    {
        var underlyingType = Nullable.GetUnderlyingType(type);
        return underlyingType is null
            ? GetNonUnderlyingTypeDefaultValueExpression(type, parameter)
            : GetUnderlyingTypeDefaultValueExpression(type, underlyingType, parameter);
    }

    public static Expression GetNonUnderlyingTypeDefaultValueExpression(Type type, ParameterInfo parameter)
    {
        if (!type.IsValueType) return Expression.Constant(parameter.DefaultValue, type);

        if (type.IsPrimitive)
        {
            var defaultValue = parameter.DefaultValue;
            if (type == typeof(nint))
                return Expression.Constant((nint)(int)defaultValue!, type);
            if (type == typeof(nuint))
                return Expression.Constant((nuint)(uint)defaultValue!, type);
            return Expression.Constant(defaultValue, type);
        }

        if (type == typeof(decimal) || type.IsEnum)
            return Expression.Constant(parameter.DefaultValue, type);
        return Expression.Default(type);
    }

    public static Expression GetUnderlyingTypeDefaultValueExpression(Type type, Type underlyingType,
        ParameterInfo parameter)
    {
        if (underlyingType.IsPrimitive)
        {
            var defaultValue = parameter.DefaultValue;
            if (underlyingType == typeof(nint))
                return Expression.Constant(defaultValue is null ? null : (nint)(int)defaultValue, type);
            if (underlyingType == typeof(nuint))
                return Expression.Constant(defaultValue is null ? null : (nuint)(uint)defaultValue, type);
            return Expression.Constant(defaultValue, type);
        }

        if (underlyingType == typeof(decimal))
            return Expression.Constant(parameter.DefaultValue, type);
        if (underlyingType.IsEnum)
            return Expression.Constant(GetUnderlyingEnumDefaultValue(underlyingType, parameter), type);
        return Expression.Default(type);
    }

    public static object? GetUnderlyingEnumDefaultValue(Type type, ParameterInfo parameter)
    {
        var defaultValue = parameter.DefaultValue;
        return defaultValue is null ? null : Enum.ToObject(type, defaultValue);
    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2067:Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.",
        Justification = "This does not actually require constructors to work")]
    private static object? GetDefaultValue(Type type)
    {
        return RuntimeHelpers.GetUninitializedObject(type);
    }
}