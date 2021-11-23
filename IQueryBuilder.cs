using System.Collections.Specialized;
using System.Linq;

namespace P.FGSP
{
    public interface IQueryBuilder
    {
        IQueryCreator Creator { get; }

        IQueryable<T> Build<T>(IQueryable<T> source, NameValueCollection form);
    }
}
