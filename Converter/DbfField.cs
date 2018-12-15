using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    public class DbfField : Field<OdbcType>
    {
        public DbfField(DataColumn column) : base(column)
        {
            NameField = column.ColumnName;
            TypeField = column.DataType;
            Lenght = column.MaxLength;
            Unique = column.Unique;
            AllowDbNull = column.AllowDBNull;
            AutoIncrement = column.AutoIncrement;
            AutoIncrementSeed = column.AutoIncrementSeed;
            AutoIncrementStep = column.AutoIncrementStep;

            TypeDictionary = InitDictionary();
        }

        public override string GenerateSql()
        {
            string sql = ConvertToDb().ToString();

            //if (AutoIncrement)
            //{
            //    sql += " AUTOINCREMENT ";// + AutoIncrementSeed + " STEP " + AutoIncrementStep+"";//"AUTOINC NEXTVALUE " + AutoIncrementSeed + " STEP " + AutoIncrementStep;
            //}

            //if (!AllowDbNull && !PrimaryKey.Contains(Column))
            //{
            //    sql += " NOT NULL";
            //}

            //if (PrimaryKey.Contains(Column))
            //{
            // sql += " PRIMARY KEY";
            //}

            //if (Unique && ! PrimaryKey.Contains(Column))
            //{
            //    sql += " UNIQUE";
            //}
            return sql;

        }

        public override Dictionary<Type, OdbcType> InitDictionary()
        {
            Dictionary<Type, OdbcType> typeMap = new Dictionary<Type, OdbcType>();
            typeMap[typeof(byte)] = OdbcType.Bit;
            typeMap[typeof(string)] = OdbcType.Char;
            typeMap[typeof(int)] = OdbcType.Int;
            typeMap[typeof(double)] = OdbcType.Double;
            ///... и т.д.
            return typeMap;
        }

    }
}
