using System;
using System.Collections.Generic;


namespace FlowManager
{
    public class FlowElement
    {
        public int id;
        public String name;
        public Boolean enabled;
        public String elementType;
        public int left;
        public int top;
        public int width;
        public int height;
        public List<Relation> relations;
    }

}
