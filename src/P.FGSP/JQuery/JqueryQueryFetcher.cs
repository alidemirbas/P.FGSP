using Microsoft.EntityFrameworkCore;

namespace P.FGSP.JQuery
{
    public class JQueryQueryFetcher : IQueryFetcher
    {
        private readonly IQueryBuilder _queryBuilder;

        public JQueryQueryFetcher(IQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task<IQueryData<T>> FetchAsync<T>(IQueryable<T> query)
        {
            var totalCount = await query.CountAsync();
            var filteredCount = await _queryBuilder.Build(query, QueryBuilderIgnore.Paging | QueryBuilderIgnore.Sorting).CountAsync();

            query = _queryBuilder.Build(query);

            var data = await query.ToArrayAsync();
            var queryData = new JQueryData<T>(data, totalCount, filteredCount);

            return queryData;
        }
    }
}
