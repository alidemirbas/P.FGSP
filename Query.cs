using System.Collections.Generic;

namespace P.FGSP
{
    public class Query:IQuery
    {
        public Query()
        {
            Sorters = new List<Sorter>();
            Groupers = new List<Grouper>();
        }
        public Pager Pager { get; set; }
        public Filter Filter { get; set; }
        public List<Sorter> Sorters { get; set; }
        public List<Grouper> Groupers { get; set; }
    }
}
