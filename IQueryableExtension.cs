using System.Collections.Specialized;
using System.Linq;

namespace P.FGSP
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> Build<T>(this IQueryable<T> source, NameValueCollection form)
        {
            IQueryCreator qc = new KendoQueryCreator();
            IQueryBuilder qb = new QueryBuilder(qc);

            return qb.Build(source, form);
        }
    }
}
