using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    //Interface for behaviour tree nodes
    public interface IParentBehaviourTreeNode : IBehaviourTreeNode
    {
        //Add a child to the parent node
        void AddChild(IBehaviourTreeNode child);
    }
}
