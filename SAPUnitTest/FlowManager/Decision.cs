using System;


namespace FlowManager
{
    public class Decision: FlowElement
    {
        public Object condition;
        public FlowElement positiveResult;
        public FlowElement negativeResult;

        public Decision()
        {
            enabled = true;
        }
    }

}
