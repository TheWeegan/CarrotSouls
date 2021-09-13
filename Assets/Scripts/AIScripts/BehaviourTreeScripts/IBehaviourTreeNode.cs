using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    //Interface for behaviour tree nodes
    public interface IBehaviourTreeNode
    {
        //Update the time of the behaviour tree
        BehaviourTreeStatus Tick(TimeData time);
    }
}
