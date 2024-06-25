using Microsoft.Extensions.Primitives;

namespace P.FGSP
{
    public abstract class QueryParametersBuilder : IQueryParametersBuilder
    {
        protected readonly IQueryParameters _queryParameters;
        protected readonly IEnumerable<KeyValuePair<string, StringValues>> _form;

        public QueryParametersBuilder(IQueryParameters queryParameters, IEnumerable<KeyValuePair<string, StringValues>> form)
        {
            _queryParameters = queryParameters;
            _form = form;
        }

        public abstract IQueryParameters Build<T>();

        protected virtual Type GetInnerMostTypeByFields(string[] fields, Type type, int depth = 0)
        {
            var t = type.GetProperties().SingleOrDefault(x => x.Name.ToLower() == fields[depth].ToLower()).PropertyType;

            if (t == null)
                return null;

            if (fields.Length != depth + 1)
            {
                var innerT = GetInnerMostTypeByFields(fields, t, depth + 1);

                if (innerT != null)
                    t = innerT;
            }

            return t;
        }
    }
}
