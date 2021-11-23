using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace P.FGSP
{

    public class KendoQueryCreator : IQueryCreator
    {
        public IQuery Create<T>(NameValueCollection form)
        {
            Type modelType = typeof(T);
            Query q = new Query();
            q.Pager = new Pager(int.Parse(form["page"]), int.Parse(form["take"]));
            q.Filter = CreateFilter(modelType, form);
            q.Sorters = CreateSorters(form);
            //todo grup

            return q;
        }

        private Filter CreateFilter(Type modelType, NameValueCollection form)
        {
            Filter filter = null;

            if (form.AllKeys.Any(item => item.StartsWith("filter[filters]")))
            {
                //q.Filters = new List<Filter>();//ctor'da yapiliyor

                int filtersCount = form.AllKeys.Where(item => item.StartsWith("filter[filters]")).Count() / 3;

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

                string fieldName;
                ConditionOperator op;
                object value;
                Type fieldType;

                for (int i = 0; i < filtersCount; i++)
                {
                    fieldName = form["filter[filters][" + i + "][field]"];//attn maksat gorunsun uzun uzun. nasil geldigi belli olsun
                    op = ConvertKendoOperatorToEnum(form["filter[filters][" + i + "][operator]"]);

                    if (!fieldName.Contains('.'))
                    {
                        fieldType = modelType.GetProperty(fieldName).PropertyType;//
                    }
                    else
                    {
                        string[] fields = fieldName.Split('.');
                        fieldType = modelType.GetProperty(fields[0]).PropertyType.GetProperty(fields[1]).PropertyType;
                    }

                    value = System.Convert.ChangeType(form["filter[filters][" + i + "][value]"], fieldType);

                    filter.Conditions.Add(new Condition(fieldName, op, value));
                }
            }

            return filter;
        }

        private List<Sorter> CreateSorters(NameValueCollection form)
        {
            List<Sorter> sorters = new List<Sorter>();

            if (form.AllKeys.Any(item => item.StartsWith("sort")))  //  x>0
            {
                int sortCount = form.AllKeys.Where(item => item.StartsWith("sort")).Count() / 2;

                ///q.Sorters = new List<Sorter>();//ctor'da yapiliyor

                string field;
                Direction dir;

                for (int i = 0; i < sortCount; i++)
                {
                    field = form["sort[" + i + "][field]"];

                    dir = ConvertKendoDirectionToEnum(form["sort[" + i + "][dir]"]);

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
                    op = default(ConditionOperator); //TODO: hiçbir case olmazsa değer ne olacak?
                    break;
            }

            return op;
        }
    }
}
