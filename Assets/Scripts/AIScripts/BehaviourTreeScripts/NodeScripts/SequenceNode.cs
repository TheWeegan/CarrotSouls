using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    //Runs child nodes in sequence, until one fails
    public class SequenceNode : IParentBehaviourTreeNode
    {
        //Name of the node.
        private string name;

        //List of child nodes.
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: this could be optimized as a baked array.

        public SequenceNode(string name)
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Success)
                {
                    return childStatus;
                }
            }

            return BehaviourTreeStatus.Success;
        }

        //Add a child to the sequence
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
