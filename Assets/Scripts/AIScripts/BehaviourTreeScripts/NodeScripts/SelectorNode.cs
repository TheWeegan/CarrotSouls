using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    //Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail
    public class SelectorNode : IParentBehaviourTreeNode
    {
        //The name of the node
        private string name;

        //List of child nodes.
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: optimization, bake this to an array.

        public SelectorNode(string name)
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Failure)
                {
                    return childStatus;
                }
            }

            return BehaviourTreeStatus.Failure;
        }

        //Add a child node to the selector
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
