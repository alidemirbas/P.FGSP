using System.Linq.Dynamic.Core;

namespace P.FGSP
{
    public class QueryBuilder : IQueryBuilder
    {
        protected IQueryParametersBuilder _queryParametersBuilder;

        public QueryBuilder(IQueryParametersBuilder queryParametersBuilder)
        {
            _queryParametersBuilder = queryParametersBuilder;
        }

        public virtual IQueryable<T> Build<T>(IQueryable<T> source, QueryBuilderIgnore? ignore = null)
        {
            IQueryParameters queryParameters = _queryParametersBuilder.Build<T>();

            var type = typeof(T);
            var fieldList = BuildPropertyCollection(type);

            if (queryParameters.Filter != null)
            {
                var ldp = BuildFilter(fieldList, queryParameters.Filter);

                if (ldp.Values.Any())
                    source = source
                        .Where(ldp.Predicate, ldp.Values.ToArray());
            }

            if (queryParameters.Sorters != null && (ignore == null || !ignore.Value.HasFlag(QueryBuilderIgnore.Sorting)))
            {
                var sortPredicate = BuildSort(queryParameters.Sorters);

                if (!string.IsNullOrEmpty(sortPredicate))
                    source = source
                        .OrderBy(sortPredicate);
            }

            if (queryParameters.Pager != null && (ignore == null || !ignore.Value.HasFlag(QueryBuilderIgnore.Paging)))
            {
                source = source
                    .Skip(queryParameters.Pager.Skip)
                    .Take(queryParameters.Pager.Take);
            }

            return source;
        }

        //recursive
        //rule SQL query'leri icin filedName, TableName.ColumnName den daha derin olmuyor o yuzden predicate icin yazacagi x.y dir.
        private LinqDynamicParameter BuildFilter(IDictionary<string, Type> fieldList, Filter filter)//, out string predicate, out object[] values)//Id=@0 and Name=@1, value=[3,"foo"] gibi
        {
            var ldp = new LinqDynamicParameter();

            if (filter == null || filter.Conditions.Count == 0)
            {
                ldp.Predicate = "true";

                return ldp;
            }

            var logic = filter.Logic.ToString().ToLower();

            foreach (var condition in filter.Conditions)
            {
                if (!string.IsNullOrEmpty(ldp.Predicate))
                    ldp.Predicate += $" {logic} ";

                ldp.Predicate += GetComparer(condition.FieldName, condition.Operator, ldp.Values.Count);

                var fieldType = fieldList.Single(item => item.Key.ToLower() == condition.FieldName.ToLower()).Value;

                ldp.Values.Add(System.Convert.ChangeType(condition.Value, fieldType));
            }

            //ldp.Predicate = $"({ldp.Predicate})";

            return ldp;
        }

        private string BuildSort(IEnumerable<Sorter> sorters)//, out object[] values) //attn orderby'da values var ama boyle de yiyor
        {
            var predicate = string.Empty;

            foreach (var sorter in sorters)
            {
                if (predicate.Length > 0)
                    predicate += ",";

                predicate += GetSort(sorter.FieldName, sorter.Direction);
            }

            return predicate;
        }

        private Dictionary<string, Type> BuildPropertyCollection(Type type)
        {
            var propertyCollection = new Dictionary<string, Type>();

            var properties = type.GetProperties();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (property.PropertyType.IsValueType || property.PropertyType == typeof(String))
                {
                    propertyCollection.Add(property.Name, property.PropertyType);
                }
                else
                {
                    var subProperties = property.PropertyType.GetProperties();

                    foreach (var subProperty in subProperties)
                    {
                        propertyCollection.Add(string.Format("{0}.{1}", property.Name, subProperty.Name), subProperty.PropertyType);
                    }
                }
            }

            return propertyCollection;
        }

        private string GetComparer(string field, ConditionOperator operatorName, int index)
        {
            string op;

            switch (operatorName)
            {
                case ConditionOperator.Equals:
                    op = "=";
                    return string.Format("{0}{1}@{2}", field, op, index); ;
                case ConditionOperator.NotEquals:
                    op = "!=";
                    return string.Format("{0}{1}@{2}", field, op, index); ;
                case ConditionOperator.GreaterThan:
                    op = ">";
                    return string.Format("{0}{1}@{2}", field, op, index); ;
                case ConditionOperator.LessThan:
                    op = "<";
                    return string.Format("{0}{1}@{2}", field, op, index); ;
                case ConditionOperator.GreaterThanOrEqual:
                    op = ">=";
                    return string.Format("{0}{1}@{2}", field, op, index); ;
                case ConditionOperator.LessThanOrEqual:
                    op = "<=";
                    return string.Format("{0}{1}@{2}", field, op, index); ;
                case ConditionOperator.Contains:
                    op = "Contains";
                    return string.Format("{0}.{1}(@{2})", field, op, index);
                case ConditionOperator.NotContain:
                    op = "Contains";
                    return string.Format("!{0}.{1}(@{2})", field, op, index);
                case ConditionOperator.StartsWith:
                    op = "StartsWith";
                    return string.Format("{0}.{1}(@{2})", field, op, index);
                case ConditionOperator.EndsWith:
                    op = "EndsWith";
                    return string.Format("{0}.{1}(@{2})", field, op, index);
                default:
                    op = "";
                    return null; //TODO:null
            }
        }//todo format yerine $"" kullan

        private string GetSort(string field, Direction direction)//,int index)//todo aslinda Direction'a gerek yok bool IsDesc olsa bitti ama ne bilim silemedim bak bakim simdi silebilecen mi?
        {
            switch (direction)
            {
                case Direction.Ascending:
                    return $"{field} asc";
                case Direction.Descending:
                    return $"{field} desc";
                default:
                    return string.Empty;
            }
        }
    }
}
