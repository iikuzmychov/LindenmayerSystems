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