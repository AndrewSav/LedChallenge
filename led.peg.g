@using System.Linq
@namespace LedChallenge
@classname LedParser
@members
{
	private int Ip;
	private Dictionary<string, int> labels = new Dictionary<string, int>();
	private void CheckLabel(string l, object cursor) 
	{
		if (!labels.ContainsKey(l)) 
		{
			FormatException ex = new FormatException($"unknown label {l}"); 
			ex.Data["cursor"] = cursor;
			throw ex;
		}
	}
}

program <IEnumerable<Action<Vm>>> = val:line* EOF {val.Where(x => x!=null)}

line <Action<Vm>> = _ val:instruction? newline #{Ip += val.Count();} {val.FirstOrDefault()} / label _ {null}

instruction <Action<Vm>> = 
	"ld" _ reg:register _ "," _ n:num _ {x => {x.WriteRegister(reg,n);}}
	/ "out" _ "(" _ i:index _ ")" _ "," _ reg:register _ {x =>{x.Output(i,reg);}}
	/ "djnz" _ lref:labelref _ #{CheckLabel(lref,cursor);} {x => {x.DecrementJump(labels[lref]);}}
	/ "rlca" _ {x => {x.RotateLeft("a");}}
	/ "rrca" _ {x => {x.RotateRight("a");}}

register = [a-b]
num <byte> = val:[0-9]<1,3> { byte.Parse(string.Join("",val)) }
index <byte> = val:[0] { byte.Parse(val) }

label = val:[a-zA-Z_]+ ":" #{labels[string.Join("",val)]=Ip;}
labelref = val:[a-zA-Z_]+ {string.Join("",val)}

_ = [ \t]*
newline = [\r\n]+

EOF
  = !.
  / unexpected:. #error{ "Error parsing" }