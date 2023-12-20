using Common;
using Day20;

void ExecuteButtonPress(Dictionary<string, Module> modules, Dictionary<string, bool> flipFlopState, Dictionary<string, Dictionary<string, bool>> conjState, Action<bool, string> onSend)
{
    Module broadcaster = modules["broadcaster"];
    Queue<(string OutputName, string InputName, bool Pulse)> toProcess = new();
    onSend(false, broadcaster.Name);
    foreach (string broadcastOutput in broadcaster.Outputs)
    {
        toProcess.Enqueue((broadcastOutput, broadcaster.Name, false));
        onSend(false, broadcastOutput);
    }

    while (toProcess.Count > 0)
    {
        (string outputName, string inputName, bool signalPulse) = toProcess.Dequeue();
        if (modules.TryGetValue(outputName, out Module outputModule))
        {
            if (outputModule.Type == ModuleType.FlipFlop && !signalPulse)
            {
                bool outputSignal = !flipFlopState[outputName];
                flipFlopState[outputModule.Name] = outputSignal;

                foreach (string newOutput in outputModule.Outputs)
                {
                    toProcess.Enqueue((newOutput, outputName, outputSignal));
                    onSend(outputSignal, newOutput);
                }
            }
            else if (outputModule.Type == ModuleType.Conjunction)
            {
                Dictionary<string, bool> state = conjState[outputModule.Name];
                state[inputName] = signalPulse;
                bool outputSignal = !state.Values.All(i => i);

                foreach (string newOutput in outputModule.Outputs)
                {
                    toProcess.Enqueue((newOutput, outputName, outputSignal));
                    onSend(outputSignal, newOutput);
                }
            }
        }
    }
}

int PulseMultiplierFor(int numPushes, Dictionary<string, Module> modules)
{
    Dictionary<bool, int> counts = new() { { true, 0 }, { false, 0 } };
    Dictionary<string, bool> flipflopState = modules.Values.Where(m => m.Type == ModuleType.FlipFlop).ToDictionary(m => m.Name, m => false);
    Dictionary<string, Dictionary<string, bool>> conjState = modules.Values.Where(m => m.Type == ModuleType.Conjunction).ToDictionary(m => m.Name, m => modules.Values.Where(inM => inM.Outputs.Contains(m.Name)).ToDictionary(inM => inM.Name, inM => false));

    for (int i = 0; i < numPushes; i++)
    {
        ExecuteButtonPress(modules, flipflopState, conjState, (signal, _) => { counts[signal]++; });
    }
    
    return counts[false] * counts[true];
}

long StepsNeededFor(string targetModule, Dictionary<string, Module> modules)
{
    Dictionary<string, bool> flipflopState = modules.Values.Where(m => m.Type == ModuleType.FlipFlop).ToDictionary(m => m.Name, m => false);
    Dictionary<string, Dictionary<string, bool>> conjState = modules.Values.Where(m => m.Type == ModuleType.Conjunction).ToDictionary(m => m.Name, m => modules.Values.Where(inM => inM.Outputs.Contains(m.Name)).ToDictionary(inM => inM.Name, inM => false));
    Module decider = modules.Values.Single(m => m.Outputs.Contains(targetModule));
    HashSet<string> toDecider = new HashSet<string>(modules.Values.Where(m => m.Outputs.Contains(decider.Name)).Select(m => m.Name));
    Dictionary<string, long> cycles = new();
    long numPushes = 0;

    while (true)
    {
        numPushes++;
        ExecuteButtonPress(modules, flipflopState, conjState, (signal, to) =>
        {
            if (toDecider.Contains(to) && !signal)
            {
                cycles.TryAdd(to, numPushes);
            }
        });

        if (cycles.Count == toDecider.Count)
        {
            return cycles.Values.Aggregate(1L, Lcm);
        }
    }
}

long Gcd(long a, long b) 
{
    if (b == 0)
    {
        return a;
    }

    return Gcd(b, a % b); 
} 
  
long Lcm(long a, long b) 
{ 
    return (a / Gcd(a, b)) * b; 
} 

string puzzleInput = await Util.GetPuzzleInput(20);

Dictionary<string, Module> modules = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Module.FromLine).ToDictionary(m => m.Name, m => m);
long pulseMultiplier = PulseMultiplierFor(1000, modules);
long stepsNeeded = StepsNeededFor("rx", modules);
Console.WriteLine($"Pulse multiplier: {pulseMultiplier}");
Console.WriteLine($"Steps needed: {stepsNeeded}");