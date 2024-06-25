using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;

namespace P.FGSP.JQuery
{

    public class JQueryQueryParametersBuilder : QueryParametersBuilder, IQueryParametersBuilder
    {
        public JQueryQueryParametersBuilder(IQueryParameters queryParameters, IEnumerable<KeyValuePair<string, StringValues>> form)
            : base(queryParameters, form)
        {

        }

        public override IQueryParameters Build<T>()
        {
            if (_form == null && !_form.Any())
                return _queryParameters;

            var formNVC = new NameValueCollection();

            foreach (var item in _form)
                formNVC.Add(item.Key, item.Value);

            var modelType = typeof(T);

            var start = int.Parse(formNVC["start"]);
            var length = int.Parse(formNVC["length"]);

            _queryParameters.Pager = new Pager((start + length) / length, length);
            _queryParameters.Filter = CreateFilter(modelType, formNVC);
            _queryParameters.Sorters = CreateSorters(formNVC);
            //todo group

            return _queryParameters;
        }

        private Filter CreateFilter(Type modelType, NameValueCollection form)
        {
            Filter filter = null;

            if (form.AllKeys.Any(item => item.StartsWith("columns")))
            {
                var filtersCount = form.AllKeys.Where(item => item.StartsWith("columns")).Count() / 6;

                filter = new Filter(default);

                for (int i = 0; i < filtersCount; i++)
                {
                    var searchable = bool.Parse(form["columns[" + i + "][searchable]"]);

                    if (!searchable)
                        continue;

                    var searchValue = form["search[value]"] ?? form["columns[" + i + "][search][value]"];

                    if (string.IsNullOrEmpty(searchValue))
                        continue;

                    var fieldName = form["columns[" + i + "][data]"];

                    var fields = fieldName.Split('.');
                    var fieldType = GetInnerMostTypeByFields(fields, modelType);

                    if (fieldType == null)
                        continue;

                    ConditionOperator op;

                    if (fieldType == typeof(string))
                        op = ConditionOperator.Contains;
                    else
                        op = ConditionOperator.Equals;

                    //fieldType Datetime olup searchValue 'foo' gibi bir değer olabilir o yüzden try
                    try
                    {
                        var value = Convert.ChangeType(searchValue, fieldType);

                        filter.Conditions.Add(new Condition(fieldName, op, value));
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }

            return filter;
        }

        private List<Sorter> CreateSorters(NameValueCollection form)
        {
            var sorters = new List<Sorter>();

            if (form.AllKeys.Any(item => item.StartsWith("order")))
            {
                int sortCount = form.AllKeys.Where(item => item.StartsWith("order")).Count() / 2;


                for (int i = 0; i < sortCount; i++)
                {
                    var fieldIndex = form["order[" + i + "][column]"];
                    var orderable = bool.Parse(form["columns[" + fieldIndex + "][orderable]"]);

                    if (!orderable)
                        continue;

                    var fieldName = form["columns[" + fieldIndex + "][data]"];
                    var dir = ConvertKendoDirectionToEnum(form["order[" + i + "][dir]"]);

                    sorters.Add(new Sorter(fieldName, dir));
                }
            }

            return sorters;
        }

        private Direction ConvertKendoDirectionToEnum(string kendoDirection)//todo
        {
            Direction dir;

            switch (kendoDirection)
            {
                case "asc":
                    dir = Direction.Ascending;
                    break;
                case "desc":
                    dir = Direction.Descending;
                    break;
                default:
                    dir = Direction.Ascending;
                    break;
            }

            return dir;
        }

        private ConditionOperator ConvertKendoOperatorToEnum(string kendoOperator)//todo
        {
            ConditionOperator op;

            switch (kendoOperator)
            {
                case "eq":
                    op = ConditionOperator.Equals;
                    break;
                case "contains":
                    op = ConditionOperator.Contains;
                    break;
                case "doesnotcontain":
                    op = ConditionOperator.NotContain;
                    break;
                case "endswith":
                    op = ConditionOperator.EndsWith;
                    break;
                case "neq":
                    op = ConditionOperator.NotEquals;
                    break;
                case "startswith":
                    op = ConditionOperator.StartsWith;
                    break;
                case "greaterThan":
                    op = ConditionOperator.GreaterThan;
                    break;
                case "lessThan":
                    op = ConditionOperator.LessThan;
                    break;
                case "greaterThanOrEqualTo":
                    op = ConditionOperator.GreaterThanOrEqual;
                    break;
                case "lessThanOrEqualTo":
                    op = ConditionOperator.LessThanOrEqual;
                    break;

                default:
                    op = default; //TODO: hiçbir case olmazsa değer ne olacak?
                    break;
            }

            return op;
        }

    }
}
