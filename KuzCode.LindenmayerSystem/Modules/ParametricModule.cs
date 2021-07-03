using System;

namespace KuzCode.LindenmayerSystem
{
    public record ParametricModuleTemplate<T>(char Symbol) : ModuleTemplate(Symbol);

    public class ParametricModule<T> : Module, ICloneable
    {
        public T Parameter { get; }

        #region Constructors
        public ParametricModule(char symbol, T parameter) : base(symbol)
        {
            Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }
        #endregion

        public override ModuleTemplate GetTemplate() => new ParametricModuleTemplate<T>(Symbol);

        public override ParametricModule<T> Clone() => new(Symbol, Parameter);

        public override string ToString() =>
            $"{Symbol}({Parameter})";

        #region Equals
        public override bool Equals(object obj)
        {
            var module = obj as ParametricModule<T>;

            if (module is null)
                return false;

            if (ReferenceEquals(this, module))
                return true;

            return base.Equals(module) && Nullable.Equals(module.Parameter, Parameter);
        }

        public static bool operator ==(ParametricModule<T> module1, ParametricModule<T> module2)
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

        public static bool operator !=(ParametricModule<T> module1, ParametricModule<T> module2)
            => !(module1 == module2);
        #endregion

        public override int GetHashCode() => (Symbol, Parameter).GetHashCode();
    }
}
