using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace P.FGSP
{
    public interface IQueryBuilder
    {
        IQueryCreator Creator { get; }

        IQueryable<T> Build<T>(IQueryable<T> source, IEnumerable<KeyValuePair<string, StringValues>> form);
    }
}
