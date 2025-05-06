namespace Application.Domain.Attributes;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class EnumNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
