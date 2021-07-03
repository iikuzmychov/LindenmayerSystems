using System;

namespace KuzCode.LindenmayerSystem
{
    public record ModuleTemplate(char Symbol);

    /// <summary>
    /// L-System smallest structural unit represented by a symbol
    /// </summary>
    public class Module : ICloneable
    {
        public char Symbol { get; }

        #region Constructors
        public Module(char symbol)
        {
            Symbol = symbol;
        }
        #endregion

        public virtual ModuleTemplate GetTemplate() => new ModuleTemplate(Symbol);

        #region Clone
        object ICloneable.Clone() => Clone();

        public virtual Module Clone() => new(Symbol);
        #endregion

        public override string ToString() => Symbol.ToString();

        #region Equals
        public override bool Equals(object obj)
        {
            var module = obj as Module;

            if (module is null)
                return false;

            if (ReferenceEquals(this, module))
                return true;

            if (GetType() != module.GetType())
                return false;

            return module.Symbol == Symbol;
        }

        public static bool operator ==(Module module1, Module module2)
        {
            if (module1 is null)
            {
                if (module2 is null)
                    return true;
                else
                    return false;
            }

            return module1.Equals(module2);
        }

        public static bool operator !=(Module module1, Module module2)
            => !(module1 == module2);
        #endregion

        public override int GetHashCode() => Symbol.GetHashCode();
    }
}
