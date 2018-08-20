using System;


namespace FlowManager
{
    public class Loop: FlowElement
    {
        public FlowElement excutionBlock;
        public Object exitCondition;

        public Loop()
        {
            enabled = true;
        }
    }

}
