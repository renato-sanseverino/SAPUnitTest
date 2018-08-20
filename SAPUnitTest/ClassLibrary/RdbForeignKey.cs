using System;


namespace ClassLibrary
{
    public class RdbForeignKey
    {
        public String name;
        public String table;
        public String referencedTable;

        public RdbForeignKey()
        {
        }

        public override string ToString()
        {
            return name;
        }
    }

}
