using System;
using System.Collections.Generic;
using System.Linq;

namespace LedChallenge
{
    public class State
    {
        private readonly Dictionary<string, byte> _registers = new Dictionary<string, byte> { { "a", 0 }, { "b", 0 } };

        public byte ReadRegister(string register)
        {
            return _registers[register];
        }

        public void WriteRegister(string register, byte val)
        {
            _registers[register] = val;
        }

        public byte DecrementRegister(string register)
        {
            return --_registers[register];            
        }

        public void RotateLeft(string register)
        {
            _registers[register] = (byte)((_registers[register] << 1) | (_registers[register] >> 7));
        }

        public void RotateRight(string register)
        {
            _registers[register] = (byte)((_registers[register] >> 1) | (_registers[register] << 7));
        }

        private string Format(string register)
        {
            return new string(
                Convert.ToString(_registers[register], 2)
                    .PadLeft(8, '0')
                    .ToCharArray()
                    .Select(x => x == '0' ? '.' : '*')
                    .ToArray());
        }
        public void Output(byte index, string register)
        {
            Console.WriteLine(Format(register));
        }
    }
}
