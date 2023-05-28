namespace P.FGSP
{
    public interface IQueryFetcher
    {
        Task<IQueryData<T>> FetchAsync<T>(IQueryable<T> query);
    }
}
