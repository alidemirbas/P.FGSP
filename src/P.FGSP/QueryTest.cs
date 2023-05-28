using Microsoft.Extensions.Primitives;
using P.FGSP.Kendo;

namespace P.FGSP
{
    public class QueryTest
    {
        public QueryTest()
        {

        }

        //let's this it's your IQueryable object like DbSet
        public static IQueryable<KendoFooDataTable> _fooQueryableDataTableResult = new List<KendoFooDataTable> {
            new KendoFooDataTable { Number = 1, Name = "a" },//bunlar gelmeli
            new KendoFooDataTable { Number = 1, Name = "b" },//bunlar gelmeli
                                                            
            new KendoFooDataTable { Number = 2, Name = "a" },
            new KendoFooDataTable { Number = 2, Name = "b" },

            new KendoFooDataTable { Number = 1, Name = "c" },
            new KendoFooDataTable { Number = 1, Name = "d" },
            new KendoFooDataTable { Number = 2, Name = "e" },
            new KendoFooDataTable { Number = 2, Name = "f" }
        }.AsQueryable();

        //and this is your http request from kendo datatable
        public static IEnumerable<KeyValuePair<string, StringValues>> _requestForm = new List<KeyValuePair<string, StringValues>> {
            new KeyValuePair<string, StringValues>("page", "1"),
            new KeyValuePair<string, StringValues>("take", "2"),

            new KeyValuePair<string, StringValues>("filter[logic]", "and"),

            new KeyValuePair<string, StringValues>("filter[filters][0][field]", "Number"),
            new KeyValuePair<string, StringValues>("filter[filters][0][operator]", "eq"),
            new KeyValuePair<string, StringValues>("filter[filters][0][value]", "1"),

            new KeyValuePair<string, StringValues>("filter[filters][1][field]", "Name"),
            new KeyValuePair<string, StringValues>("filter[filters][1][operator]", "eq"),
            new KeyValuePair<string, StringValues>("filter[filters][1][value]", "a"),

            new KeyValuePair<string, StringValues>("sort[0][field]", "Number"),
            new KeyValuePair<string, StringValues>("sort[0][dir]", "asc"),
        };

        public void Test1()
        {
            var queryParameters = new QueryParameters();
            var queryParametersCreator = new KendoQueryParametersBuilder(queryParameters, _requestForm);
            var queryBuilder = new QueryBuilder(queryParametersCreator);
            var fetcher = new KendoQueryFetcher(queryBuilder);
            ;
            var data = fetcher.FetchAsync(_fooQueryableDataTableResult);
        }
    }

    public class KendoFooDataTable
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }
}
