using System;

namespace KuzCode.LindenmayerSystem
{
    /// <summary>
    /// Module with 1 parameter
    /// </summary>
    public record ParametricModule<TParameter>(char Symbol, TParameter? Parameter) : Module(Symbol)
        where TParameter : struct
    {
        public ParametricModule(char symbol, TParameter parameter) : this(symbol, (TParameter?)parameter) { }

        public override bool IsUseableAsTemplateOnly => !Parameter.HasValue;

        public sealed override bool IsMatchTemplate(Module template)
            => IsMatchTemplate(template as ParametricModule<TParameter>);

        public bool IsMatchTemplate(ParametricModule<TParameter> template)
        {
            if (template is null)
                throw new ArgumentNullException(nameof(template));

            if (template.Parameter.HasValue)
                return Equals(template);
            else
                return (this with { Parameter = null }).Equals(template);
        }

        public override string ToString() => $"{Symbol}({Parameter})";
    }
}
