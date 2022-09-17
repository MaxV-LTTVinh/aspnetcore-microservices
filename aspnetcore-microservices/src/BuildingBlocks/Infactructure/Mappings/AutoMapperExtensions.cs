using System.Reflection;
using AutoMapper;

namespace Infrastructure.Mappings;

public static class AutoMapperExtension
{
    public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
        (this IMappingExpression<TSource, TDestination> expression)
    {
        var flags = BindingFlags.Public | BindingFlags.Instance;
        var sourceType = typeof(TSource);
        var destinationProperties = typeof(TDestination).GetProperties(flags);

        foreach (var propertyName in destinationProperties.Select(e => e.Name))
            if (sourceType.GetProperty(propertyName, flags) == null)
                expression.ForMember(propertyName, opt => opt.Ignore());
        return expression;
    }
}