using System;
using System.IO;
using System.Collections.Generic;

namespace LedChallenge
{
    static class Entry
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("Usage: LedChallenge inputFile");
                }
                LedParser parser = new LedParser();
                string text = File.ReadAllText(args[0]);
                IList<Action<Vm, State>> program = parser.Parse(text);
                Vm vm = new Vm {Program = program,State = new State()};
                while (!vm.Eof())
                {
                    vm.Step();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
        }
    }
}
