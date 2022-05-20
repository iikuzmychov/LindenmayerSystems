namespace KuzCode.LindenmayerSystems.Benchmarks;

public class NamedLSystem : LSystem
{
    public string Name { get; }

    public NamedLSystem(string name, Module[] axiom, IEnumerable<IProduction<Module>> productions)
        : base(axiom, productions)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public override string ToString() => Name;
}
