using Common;
using Day20;

int Execute(int numPushes, Dictionary<string, Module> modules)
{
    Dictionary<bool, int> counts = new() { { true, 0 }, { false, 0 } };
    Dictionary<string, bool> flipflopState = modules.Values.Where(m => m.Type == ModuleType.FlipFlop).ToDictionary(m => m.Name, m => false);
    Dictionary<string, Dictionary<string, bool>> conjState = modules.Values.Where(m => m.Type == ModuleType.Conjunction).ToDictionary(m => m.Name, m => modules.Values.Where(inM => inM.Outputs.Contains(m.Name)).ToDictionary(inM => inM.Name, inM => false));
    var broadcaster = modules["broadcaster"];

    for (int i = 0; i < numPushes; i++)
    {
        Queue<(string OutputName, string InputName, bool Pulse)> toProcess = new();
        counts[false]++;
        foreach (string broadcastOutput in broadcaster.Outputs)
        {
            toProcess.Enqueue((broadcastOutput, broadcaster.Name, false));
            counts[false]++;
        }

        while (toProcess.Count > 0)
        {
            (string outputName, string inputName, bool signalPulse) = toProcess.Dequeue();
            if (modules.TryGetValue(outputName, out Module outputModule))
            {
                if (outputModule.Type == ModuleType.FlipFlop)
                {
                    if (!signalPulse)
                    {
                        bool outputState = flipflopState[outputName];
                        bool outputSignal = !outputState;
                        bool newState = !outputState;
                        flipflopState[outputModule.Name] = newState;

                        foreach (string newOutput in outputModule.Outputs)
                        {
                            toProcess.Enqueue((newOutput, outputName, outputSignal));
                            counts[outputSignal]++;
                        }
                    }
                }
                else if (outputModule.Type == ModuleType.Conjunction)
                {
                    conjState[outputModule.Name][inputName] = signalPulse;
                    bool outputSignal = conjState[outputModule.Name].Values.All(i => i) ? false : true;

                    foreach (string newOutput in outputModule.Outputs)
                    {
                        toProcess.Enqueue((newOutput, outputName, outputSignal));
                        counts[outputSignal]++;
                    }
                }
            }
        }
    }
    
    return counts[false] * counts[true];
}

string puzzleInput = await Util.GetPuzzleInput(20);

var modules = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Module.FromLine).ToDictionary(m => m.Name, m => m);
Console.WriteLine(string.Join(Environment.NewLine, modules.Values));
Console.WriteLine(Execute(1000, modules));