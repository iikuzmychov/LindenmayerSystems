using System;

namespace KuzCode.LindenmayerSystem;

/*public abstract class ParametricModule : Module
{
    public ParametricModule(string name) : base(name) { }
}*/

/// <summary>
/// <see cref="Module"/> with 1 parameter.
/// </summary>
public class ParametricModule<T> : Module, ICloneable, IEquatable<ParametricModule<T>>
{
    public T Parameter { get; }

    public ParametricModule(string name, T parameter) : base(name)
    {
        Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
    }

    public override object Clone() => new ParametricModule<T>(Name, Parameter);

    #region Comparing
    public bool Equals(ParametricModule<T>? otherModule)
    {
        if (!base.Equals(otherModule))
            return false;

        return Parameter!.Equals(otherModule.Parameter);
    }

    public override bool Equals(object? obj) => Equals(obj as ParametricModule<T>);

    public static bool operator ==(ParametricModule<T> module1, ParametricModule<T> module2)
    {
        if (module1 is null)
            return module2 is null;
        else
            return module1.Equals(module2);
    }

    public static bool operator !=(ParametricModule<T> module1, ParametricModule<T> module2) => !(module1 == module2);

    public override int GetHashCode() => Name.GetHashCode();
    #endregion

    public override string ToString() => $"{Name}({Parameter})";
}
