using System;
using System.Collections.Generic;

namespace KuzCode.LindenmayerSystem;

public interface IProducer<out TModule>
    where TModule : Module
{
    Type GetInputModuleType() => typeof(TModule);

    List<Module> Produce(Module module, ProductionContext context);
}
