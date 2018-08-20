using System;


namespace ClassLibrary
{
    public class RdbField
    {
        public String name;
        public String type;
        public short size;

        public RdbField()
        {
        }

        public override string ToString()
        {
            return name + " " + type + "(" + size + ")";
        }
    }

}
