namespace MySql.DataLayer.Core
{
    public class ColumnTable
    {
       private string _columnName;

       public ColumnTable(string columnName)
       {
           _columnName = columnName;
       }

        public ColumnTable As (string alias) {
            _columnName += $" as `{alias}`";
            return this;
        }


        public override string ToString () {
            return _columnName;
        }
    }
}