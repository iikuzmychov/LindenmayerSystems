using System;

namespace KuzCode.LindenmayerSystems;

/// <summary>
/// The smallest structural unit of the <see cref="LSystem"/> represented by the symbol.
/// </summary>
public record Module
{
    public char Symbol { get; }

    public Module(char symbol)
    {
        Symbol = symbol;
    }

    public override string ToString() => Symbol.ToString();
}

/// <summary>
/// <see cref="Module"/> with a parameter of type <typeparamref name="T"/>.
/// </summary>
public record Module<T> : Module
{
    public T Parameter { get; }

    public Module(char symbol, T parameter) : base(symbol)
    {
        Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
    }

    public override string ToString() => $"{Symbol}({Parameter})";
}