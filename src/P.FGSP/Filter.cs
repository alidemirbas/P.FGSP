namespace P.FGSP
{
    public class Filter
    {
        public Filter(Logic logic)
        {
            Logic = logic;
            Conditions = new List<Condition>();
        }

        public Logic Logic { get; set; }
        public List<Condition> Conditions { get; set; }
    }

    public class Condition
    {
        public Condition(string fieldName, ConditionOperator opr, object value)
        {
            FieldName = fieldName;
            Operator = opr;
            Value = value;
        }

        public string FieldName { get; }
        public ConditionOperator Operator { get; }
        public object Value { get; }
    }

    public class LinqDynamicParameter
    {
        public LinqDynamicParameter()
        {
            Values = new List<object>();
        }

        public string Predicate { get; set; }
        public List<object> Values { get; set; }
    }
}
