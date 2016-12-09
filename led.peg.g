@using System.Linq
@namespace LedChallenge
@classname LedParser
@members
{
	private int IP;
	private Dictionary<string, int> labels = new Dictionary<string, int>();
	private List<Action<Vm,State>> empty = new List<Action<Vm,State>>();
	private Action<Vm,State> djnz(string lref)
	{
		return (vm, st) => {
			if (st.DecrementRegister("b") > 0) {
			    vm.IP = labels[lref] - 1;
			}
		};
	}
}

program <IList<Action<Vm,State>>> = i:line* EOF {i.SelectMany(x => x).ToList()}

line <IList<Action<Vm,State>>> = _ i:instruction? newline #{IP += i.Count();} {i} / label _ {empty}

instruction <Action<Vm,State>> = 
	"ld" _ reg:register _ "," _ n:num _ {(vm, st) => {st.WriteRegister(reg,n);}}
	/ "out" _ "(" _ i:index _ ")" _ "," _ reg:register _ {(vm, st) =>{st.Output(i,reg);}}
	/ "djnz" _ lref:labelref _ {djnz(lref)}
	/ "rlca" _ {(vm, st) => {st.RotateLeft("a");}}
	/ "rrca" _ {(vm, st) => {st.RotateRight("a");}}

register = [a-b]
num <byte> = value:[0-9]<1,3> { byte.Parse(string.Join("",value)) }
index <byte> = value:[0] { byte.Parse(value) }

label = value:[a-zA-Z_]+ ":" #{labels[string.Join("",value)]=IP;}
labelref = value:[a-zA-Z_]+ {string.Join("",value)}

_ = [ \t]*
newline = [\r\n]+

EOF
  = !.
  / unexpected:. #error{ "Unexpected character '" + unexpected + "'." }