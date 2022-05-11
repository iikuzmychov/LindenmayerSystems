using KuzCode.LindenmayerSystems;
using KuzCode.LindenmayerSystems.Extensions;
using System.Diagnostics;
using System.Text;

var perfomanceTestLSystem = new LSystem(
    axiom:       new Module[] { new('F') },
    productions: new IProduction<Module>[]
    {
        new RegularProductionBuilder<Module>()
            .SetPredecessorPredicate(predecessor => predecessor.Symbol == 'F')
            .SetSuccessors(new('F'), new('+'), new('G'))
            .Build(),

        new RegularProductionBuilder<Module>()
            .SetPredecessorPredicate(predecessor => predecessor.Symbol == 'G')
            .SetSuccessors(new('F'), new('-'), new('G'))
            .Build(),
    });

var perfomanceTestFakeLSystem = new FakeLSystem(
    axiom:       new Module[] { new('F') },
    productions: new()
    {
        { new('F'), (ICollection<Module>)"F+G".ParseAsModules() },
        { new('G'), (ICollection<Module>)"F-G".ParseAsModules() }
    });

for (int i = 0; i < 3; i++)
{
    perfomanceTestLSystem.Reset();

    var stopwatch = Stopwatch.StartNew();
    perfomanceTestLSystem.NextSteps(15);
    stopwatch.Stop();

    Console.WriteLine(stopwatch.Elapsed.ToString());
}

Console.WriteLine();

for (int i = 0; i < 3; i++)
{
    perfomanceTestFakeLSystem.Reset();

    var stopwatch = Stopwatch.StartNew();
    perfomanceTestFakeLSystem.NextSteps(15);
    stopwatch.Stop();

    Console.WriteLine(stopwatch.Elapsed.ToString());
}

public class FakeLSystem
{
    private Dictionary<Module, ICollection<Module>> _productions;

    public Module[] Axiom { get; }
    public IList<Module> State { get; private set; }

    public FakeLSystem(Module[] axiom, Dictionary<Module, ICollection<Module>> productions)
    {
        Axiom        = axiom ?? throw new ArgumentNullException(nameof(axiom));
        State        = Axiom;
        _productions = productions ?? throw new ArgumentNullException(nameof(productions));
    }

    public void Reset()
    {
        State = Axiom;
    }

    public IList<Module> NextStep()
    {
        var newState = new List<Module>(State.Count);

        foreach (var module in State)
        {
            if (_productions.ContainsKey(module))
                newState.AddRange(_productions[module]);
            else
                newState.Add(module);
        }

        State = newState;

        return State;
    }

    public IList<Module> NextSteps(int stepsCount)
    {
        if (stepsCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(stepsCount));

        for (int i = 0; i < stepsCount; i++)
            NextStep();

        return State;
    }
}