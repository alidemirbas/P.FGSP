namespace P.FGSP.Kendo
{
    public class KendoData<T> : IQueryData<T>
    {
        public KendoData(IEnumerable<T> data)
        {
            Data = data;
        }

        public IEnumerable<T> Data { get; }
        public long Total => Data.Count();
    }
}
