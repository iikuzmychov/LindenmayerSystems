using System;

namespace KuzCode.LindenmayerSystems;

/// <summary>
/// <see cref="Module"/> with a parameter of type <typeparamref name="T"/>.
/// </summary>
public record ParametricModule<T> : Module
{
    public T Parameter { get; }

    public ParametricModule(char symbol, T parameter) : base(symbol)
    {
        Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
    }

    public override string ToString() => $"{Symbol}({Parameter})";
}
