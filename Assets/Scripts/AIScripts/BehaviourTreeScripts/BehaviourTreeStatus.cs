using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    //The return type when invoking behaviour tree nodes
    public enum BehaviourTreeStatus
    {
        Success,
        Failure,
        Running
    }
}
