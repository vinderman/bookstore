using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace Bookstore.WebApi.Common;

public class SwaggerEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            // Устанавливаем тип схемы как строку
            schema.Type = "string";
            schema.Format = null; // Убираем формат

            // Добавляем возможные значения Enum как строки
            schema.Enum = Enum.GetNames(context.Type)
                .Select(name => new OpenApiString(name))
                .ToList<IOpenApiAny>();

            // Добавляем описания для значений Enum
            schema.Description = "Возможные значения:\n" + string.Join("\n", Enum.GetNames(context.Type)
                .Select(name => $"- {name}: {GetEnumDescription(context.Type, name)}"));
        }
    }

    private static string GetEnumDescription(Type type, string name)
    {
        var field = type.GetField(name);
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? "Нет описания";
    }
}
