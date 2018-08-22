namespace MySql.DataLayer.Core
{
    public enum Condition
    {
        Where,
        And,
        Or
    }
    public enum ConditionOperator
    {
        Equal,
        Diff,
        Gt,
        Gte,
        Lt,
        Lte,
        NotNull,
        Isnull
    }
    public class ConditionSearch
    {
        public string ParameterName { get; set; }
        public object ParameterValue { get; set; }
        public Condition Condition { get; set; }
        public ConditionOperator Operator { get; set; }

    }
}