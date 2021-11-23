using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P.FGSP
{
    public class Sorter
    {
        public Sorter(string fieldName,Direction direction)
        {
            FieldName = fieldName;
            Direction = direction;
        }

        public string FieldName { get; }
        public Direction Direction { get; }
    }

   
}
