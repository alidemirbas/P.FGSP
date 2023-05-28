namespace P.FGSP.JQuery
{
    public class JQueryData<T> : IQueryData<T>
    {
        public JQueryData(IEnumerable<T> data, long recordsTotal, long recordsFiltered)
        {
            Data = data;
            RecordsTotal = recordsTotal;
            RecordsFiltered = recordsFiltered;
        }

        public IEnumerable<T> Data { get; }
        public long RecordsTotal { get; }
        public long RecordsFiltered { get; }
    }
}
