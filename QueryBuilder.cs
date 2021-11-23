using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace P.FGSP
{
    public class QueryBuilder : IQueryBuilder
    {
        public QueryBuilder(IQueryCreator creator)
        {
            Creator = creator;
        }

        public IQueryCreator Creator { get; }

        public IQueryable<T> Build<T>(IQueryable<T> source, IEnumerable<KeyValuePair<string, StringValues>> form)
        {
            IQuery query = Creator.Create<T>(form);

            Type type = typeof(T);
            Dictionary<string, Type> fieldList = BuildPropertyCollection(type);


            LinqDynamicParameter ldp = BuildFilter(fieldList, query.Filter);
            string sortPredicate = BuildSort(query.Sorters);

            return source
                .Where(ldp.Predicate, ldp.Values.ToArray())
                .OrderBy(sortPredicate)
                .Skip(query.Pager.Skip)
                .Take(query.Pager.Take);
        }

        //recursive
        //rule SQL query'leri icin filedName, TableName.ColumnName den daha derin olmuyor o yuzden predicate icin yazacagi x.y dir.
        private LinqDynamicParameter BuildFilter(IDictionary<string, Type> fieldList, Filter filter)//, out string predicate, out object[] values)//Id=@0 and Name=@1, value=[3,"foo"] gibi
        {
            LinqDynamicParameter ldp = new LinqDynamicParameter();

            if (filter == null || filter.Conditions.Count == 0)
            {
                ldp.Predicate = "true";

                return ldp;
            }

            Type fieldType;
            string logic = filter.Logic.ToString().ToLower();

            foreach (var condition in filter.Conditions)
            {
                if (!string.IsNullOrEmpty(ldp.Predicate))
                    ldp.Predicate += $" {logic} ";

                ldp.Predicate += GetComparer(condition.FieldName, condition.Operator, ldp.Values.Count);

                fieldType = fieldList.Single(item => item.Key == condition.FieldName).Value;

                ldp.Values.Add(System.Convert.ChangeType(condition.Value, fieldType));
            }

            //ldp.Predicate = $"({ldp.Predicate})";

            return ldp;
        }

        private string BuildSort(IEnumerable<Sorter> sorters)//, out object[] values) //attn orderby'da values var ama boyle de yiyor
        {
            string predicate = string.Empty;
            //values = new object[0];

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
            Dictionary<string, Type> propertyCollection = new Dictionary<string, Type>();

            System.Reflection.PropertyInfo[] properties = type.GetProperties();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (property.PropertyType.IsValueType || property.PropertyType == typeof(String))
                {
                    propertyCollection.Add(property.Name, property.PropertyType);
                }
                else
                {
                    System.Reflection.PropertyInfo[] subProperties = property.PropertyType.GetProperties();

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
