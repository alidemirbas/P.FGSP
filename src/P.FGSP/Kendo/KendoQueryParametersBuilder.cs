using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;

namespace P.FGSP.Kendo
{

    public class KendoQueryParametersBuilder : QueryParametersBuilder, IQueryParametersBuilder
    {
        public KendoQueryParametersBuilder(IQueryParameters queryParameters, IEnumerable<KeyValuePair<string, StringValues>> form)
            : base(queryParameters, form)
        {

        }

        public override IQueryParameters Build<T>()
        {
            var formNVC = new NameValueCollection();

            foreach (var item in _form)
                formNVC.Add(item.Key, item.Value);

            var modelType = typeof(T);

            _queryParameters.Pager = new Pager(int.Parse(formNVC["page"]), int.Parse(formNVC["take"]));
            _queryParameters.Filter = CreateFilter(modelType, formNVC);
            _queryParameters.Sorters = CreateSorters(formNVC);
            //todo group

            return _queryParameters;
        }

        private Filter CreateFilter(Type modelType, NameValueCollection form)
        {
            Filter filter = null;

            if (form.AllKeys.Any(item => item.StartsWith("filter[filters]")))
            {
                //q.Filters = new List<Filter>();//ctor'da yapiliyor

                var filtersCount = form.AllKeys.Where(item => item.StartsWith("filter[filters]")).Count() / 3;

                Logic logic = default;

                var logicNames = Enum.GetNames(typeof(Logic));

                foreach (var n in logicNames)
                {
                    if (n.ToLower() == form["filter[logic]"].ToLower())
                    {
                        logic = (Logic)Enum.Parse(typeof(Logic), n);
                        break;
                    }
                }

                filter = new Filter(logic);
                //todo kendo icin +veya oluyor mu arastir ona gore filterItemslari duzenle

                for (int i = 0; i < filtersCount; i++)
                {
                    var fieldName = form["filter[filters][" + i + "][field]"];//attn maksat gorunsun uzun uzun. nasil geldigi belli olsun
                    var op = ConvertKendoOperatorToEnum(form["filter[filters][" + i + "][operator]"]);

                    var fields = fieldName.Split('.');
                    var fieldType = GetInnerMostTypeByFields(fields, modelType);
                    var value = Convert.ChangeType(form["filter[filters][" + i + "][value]"], fieldType);

                    filter.Conditions.Add(new Condition(fieldName, op, value));
                }
            }

            return filter;
        }

        private List<Sorter> CreateSorters(NameValueCollection form)
        {
            var sorters = new List<Sorter>();

            if (form.AllKeys.Any(item => item.StartsWith("sort")))  //  x>0
            {
                var sortCount = form.AllKeys.Where(item => item.StartsWith("sort")).Count() / 2;

                ///q.Sorters = new List<Sorter>();//ctor'da yapiliyor

                for (int i = 0; i < sortCount; i++)
                {
                    var field = form["sort[" + i + "][field]"];
                    var dir = ConvertKendoDirectionToEnum(form["sort[" + i + "][dir]"]);

                    sorters.Add(new Sorter(field, dir));
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
