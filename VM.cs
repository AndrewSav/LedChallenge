// ReSharper disable UnusedMember.Global
using System;
using System.Collections.Generic;
using System.Linq;

namespace LedChallenge
{
    public class Vm
    {
        public Vm(IList<Action<Vm>> program)
        {
            _program = program;
        }
        private readonly Dictionary<string, byte> _registers = new Dictionary<string, byte> { { "a", 0 }, { "b", 0 } };
        private readonly IList<Action<Vm>> _program;
        private int _ip;

        public void Step()
        {
            _program[_ip](this);
            _ip++;
        }

        public bool Eof()
        {
            return _ip >= _program.Count;
        }

        public byte ReadRegister(string register)
        {
            return _registers[register];
        }

        public void WriteRegister(string register, byte val)
        {
            _registers[register] = val;
        }

        private byte DecrementRegister(string register)
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

        public void DecrementJump(int offset)
        {
            if (DecrementRegister("b") > 0)
            {
                _ip = offset - 1;
            }
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
