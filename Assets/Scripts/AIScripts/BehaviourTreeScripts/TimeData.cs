using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree {
    //Represents time. Used to pass time values to behaviour tree nodes
    public struct TimeData {
        public TimeData(float deltaTime) {
            this.deltaTime = deltaTime;
        }
        public float deltaTime;
    }
}
