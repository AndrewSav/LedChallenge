using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Pegasus.Common;

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
                if (e.Data.Contains("cursor") && e.Data["cursor"] is Cursor)
                {
                    Cursor c = (Cursor)e.Data["cursor"];
                    Console.WriteLine($"in {args[0]} at: Line:{c.Line}, Column:{c.Column}");
                }
            }
        }
    }
}
