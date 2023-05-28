namespace P.FGSP
{
    public interface IQueryData<T>
    {
        IEnumerable<T> Data { get; }
    }
}
