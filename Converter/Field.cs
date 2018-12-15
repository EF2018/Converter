using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    abstract public class Field<T> where T : new()
    {
        public Dictionary<Type, T> TypeDictionary { get; set; }
        public string NameField { get; set; }
        public Type TypeField { get; set; }
        public int Lenght { get; set; }
        public bool AutoIncrement { get; set; }
        public bool AllowDbNull { get; set; }
        public long AutoIncrementSeed { get; set; }
        public long AutoIncrementStep { get; set; }
        public bool Unique { get; set; }
        public DataColumn[] PrimaryKey { get; set; }
        public object DefaultValue { get; set; }
        public DataColumn Column { get; set; }
        public Field(DataColumn column)
        {
            Column = column;
            NameField = column.ColumnName;
            TypeField = column.DataType;
            Lenght = column.MaxLength;
            Unique = column.Unique;
            AllowDbNull = column.AllowDBNull;
            AutoIncrement = column.AutoIncrement;
            AutoIncrementSeed = column.AutoIncrementSeed;
            AutoIncrementStep = column.AutoIncrementStep;
            PrimaryKey = column.Table.PrimaryKey;

            TypeDictionary = InitDictionary();
        }

        public virtual string GenerateSql() { return ""; }
        public virtual Dictionary<Type, T> InitDictionary()
        {
            return new Dictionary<Type, T>();
        }

        public virtual T ConvertToDb()
        {
            return TypeDictionary[TypeField];
        }

    }
}
