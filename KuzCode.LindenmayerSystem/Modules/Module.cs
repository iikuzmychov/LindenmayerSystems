using System;

namespace KuzCode.LindenmayerSystem
{
    /// <summary>
    /// <see cref="LSystem"/> smallest structural unit.
    /// </summary>
    public class Module : ICloneable, IEquatable<Module>
    {
        public string Name { get; }

        public Module(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public virtual object Clone() => new Module(Name);

        #region Comparing
        public bool Equals(Module? otherModule)
        {
            if (otherModule is null)
                return false;

            if (ReferenceEquals(this, otherModule))
                return true;

            return otherModule.Name == Name;
        }

        public override bool Equals(object? obj) => Equals(obj as Module);

        public static bool operator==(Module module1, Module module2)
        {
            if (module1 is null)
                return module2 is null;
            else
                return module1.Equals(module2);
        }

        public static bool operator !=(Module module1, Module module2) => !(module1 == module2);

        public override int GetHashCode() => Name.GetHashCode();
        #endregion

        public override string ToString() => Name;
    }
}
