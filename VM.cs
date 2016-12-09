using System;
using System.Collections.Generic;

namespace LedChallenge
{
    public class Vm
    {
        public Vm()
        {
            Program = new List<Action<Vm,State>>();
        }
        public IList<Action<Vm,State>> Program { get; set; }
        public int IP;
        public State State { get; set; }

        public void Step()
        {
            Program[IP](this, State);
            IP++;
        }

        public bool Eof()
        {
            return IP >= Program.Count;
        }
    }
}
