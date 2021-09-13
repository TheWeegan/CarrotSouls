using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    //A behaviour tree leaf node for running an action
    public class ActionNode : IBehaviourTreeNode
    {
        //The name of the node
        private string name;

        //Function to invoke for the action.
        private Func<TimeData, BehaviourTreeStatus> fn;
        
        public ActionNode(string name, Func<TimeData, BehaviourTreeStatus> fn)
        {
            this.name=name;
            this.fn=fn;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            return fn(time);
        }
    }
}
