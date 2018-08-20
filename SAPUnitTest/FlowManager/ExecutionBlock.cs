using System;
using System.Collections.Generic;


namespace FlowManager
{
    public class ExecutionBlock: FlowElement
    {
        public List<Object> task;

        public ExecutionBlock()
        {
            enabled = true;
            task = new List<Object>();
        }
    }

}
