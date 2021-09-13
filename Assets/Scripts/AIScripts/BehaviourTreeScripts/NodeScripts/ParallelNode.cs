using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    //Runs childs nodes in parallel
    public class ParallelNode : IParentBehaviourTreeNode
    {
        //Name of the node
        private string name;

        //List of child nodes
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>();

        //Number of child failures required to terminate with failure
        private int numRequiredToFail;

        //Number of child successess require to terminate with success
        private int numRequiredToSucceed;

        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed)
        {
            this.name = name;
            this.numRequiredToFail = numRequiredToFail;
            this.numRequiredToSucceed = numRequiredToSucceed;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            var numChildrenSuceeded = 0;
            var numChildrenFailed = 0;

            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                switch (childStatus)
                {
                    case BehaviourTreeStatus.Success: ++numChildrenSuceeded; break;
                    case BehaviourTreeStatus.Failure: ++numChildrenFailed; break;
                }
            }

            if (numRequiredToSucceed > 0 && numChildrenSuceeded >= numRequiredToSucceed)
            {
                return BehaviourTreeStatus.Success;
            }

            if (numRequiredToFail > 0 && numChildrenFailed >= numRequiredToFail)
            {
                return BehaviourTreeStatus.Failure;
            }

            return BehaviourTreeStatus.Running;
        }

        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
