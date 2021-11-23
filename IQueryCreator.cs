using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace P.FGSP
{
    public interface IQueryCreator
    {
        IQuery Create<T>(IEnumerable<KeyValuePair<string, StringValues>> form);
    }
}
