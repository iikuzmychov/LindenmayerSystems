using System;

namespace KuzCode.LindenmayerSystem
{
    /// <summary>
    /// L-System smallest structural unit represented by a symbol
    /// </summary>
    public record Module(char Symbol)
    {
        public virtual bool IsUseableAsTemplateOnly => false;

        public virtual bool IsMatchTemplate(Module template)
        {
            if (template is null)
                throw new ArgumentNullException(nameof(template));

            return Equals(template);
        }

        public override string ToString() => Symbol.ToString();
    }
}
