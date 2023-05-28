using Microsoft.EntityFrameworkCore;

namespace P.FGSP.Kendo
{
    public class KendoQueryFetcher : IQueryFetcher
    {
        private readonly IQueryBuilder _queryBuilder;

        public KendoQueryFetcher(IQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task<IQueryData<T>> FetchAsync<T>(IQueryable<T> query)
        {
            var totalCount = await query.CountAsync();

            query = _queryBuilder.Build(query);

            var data = await query.ToArrayAsync();
            var queryData = new KendoData<T>(data);

            return queryData;
        }
    }
}
