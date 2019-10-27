using System;
using System.Threading.Tasks;

namespace movement_simulator
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            int stepCount = int.Parse(args[0]);
            var simulator = new Simulator();
            await simulator.Initialize();
            for (int i = 0; i < stepCount; i++)
                await simulator.Step();
            return 0;
        }
    }
}
