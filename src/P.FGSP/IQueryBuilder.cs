namespace P.FGSP
{
    public interface IQueryBuilder
    {
        IQueryable<T> Build<T>(IQueryable<T> source, QueryBuilderIgnore? ignore = null);
    }
}
