namespace CliWithSpectreConsole.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

public sealed class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection builder;

    public TypeRegistrar(IServiceCollection builder) =>
        this.builder = builder;

    public ITypeResolver Build() =>
        new TypeResolver(this.builder.BuildServiceProvider());

    public void Register(Type service, Type implementation) => this.builder.AddSingleton(service, implementation);

    public void RegisterInstance(Type service, object implementation) =>
        this.builder.AddSingleton(service, implementation);

    public void RegisterLazy(Type service, Func<object> factory)
    {
        if (factory is null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        this.builder.AddSingleton(service, _ => factory());
    }
}
