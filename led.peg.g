@using System.Linq
@namespace LedChallenge
@classname LedParser
@members
{
	private List<Action<Vm>> empty = new List<Action<Vm>>();

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

program <IList<Action<Vm>>> = i:line* EOF {i.SelectMany(x => x).ToList()}

line <IList<Action<Vm>>> = _ i:instruction? newline #{Ip += i.Count();} {i} / label _ {empty}

instruction <Action<Vm>> = 
	"ld" _ reg:register _ "," _ n:num _ {x => {x.WriteRegister(reg,n);}}
	/ "out" _ "(" _ i:index _ ")" _ "," _ reg:register _ {x =>{x.Output(i,reg);}}
	/ "djnz" _ lref:labelref _ #{CheckLabel(lref,cursor);} {x => {x.DecrementJump(labels[lref]);}}
	/ "rlca" _ {x => {x.RotateLeft("a");}}
	/ "rrca" _ {x => {x.RotateRight("a");}}

register = [a-b]
num <byte> = value:[0-9]<1,3> { byte.Parse(string.Join("",value)) }
index <byte> = value:[0] { byte.Parse(value) }

label = value:[a-zA-Z_]+ ":" #{labels[string.Join("",value)]=Ip;}
labelref = value:[a-zA-Z_]+ {string.Join("",value)}

_ = [ \t]*
newline = [\r\n]+

EOF
  = !.
  / unexpected:. #error{ "Error parsing" }