using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stateless;
using Stateless.Graph;

namespace hello
{
    class Program
    {
        enum Trigger { TOGGLE };
        enum State { ON, OFF };
        static StateMachine<State, Trigger> sm1;

        static bool IsLightNeeded() {
            return false;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, world!");
            State CurrentState = State.OFF;
            sm1=new StateMachine<State, Trigger>(() => CurrentState, s => CurrentState = s);
            sm1.Configure(State.ON).Permit(Trigger.TOGGLE, State.OFF);

            sm1.Configure(State.OFF).PermitIf(Trigger.TOGGLE, State.ON, () => IsLightNeeded(),"Toggle allowed")
                    .PermitReentryIf(Trigger.TOGGLE, () => !IsLightNeeded(),"Toggle not allowed");

            sm1.Fire(Trigger.TOGGLE);
            string graph = UmlDotGraph.Format(sm1.GetInfo());
            Console.WriteLine(graph);
        }
    }
}