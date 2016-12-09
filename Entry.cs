using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
                IList<Action<Vm>> program = parser.Parse(File.ReadAllText(args[0])).ToList();
                Vm vm = new Vm(program);
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
