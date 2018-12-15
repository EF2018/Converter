using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    public class SqlField : Field<SqlDbType>
    {
        public SqlField(DataColumn column) : base(column)//string namefield, Type data_type, int lenght, string source, bool nullpermishion) : base(namefield, data_type, lenght, source, nullpermishion)
        {
            NameField = column.ColumnName;
            TypeField = column.DataType;
            Lenght = column.MaxLength;
            TypeDictionary = InitDictionary();
        }

        public override Dictionary<Type, SqlDbType> InitDictionary()
        {
            Dictionary<Type, SqlDbType> typeMap = new Dictionary<Type, SqlDbType>();
            typeMap[typeof(byte)] = SqlDbType.Bit;
            typeMap[typeof(string)] = SqlDbType.VarChar;
            typeMap[typeof(int)] = SqlDbType.Int;
            typeMap[typeof(double)] = SqlDbType.Float;

            ///...и т.д.
            return typeMap;
        }

        public override string GenerateSql()
        {
            string sql = ConvertToDb().ToString();

            if (Lenght > 0)
            {
                sql += String.Format(" ({0})", Lenght);
            }

            if (AutoIncrement)
            {
                sql += String.Format(" IDENTITY({0},{1})", AutoIncrementSeed, AutoIncrementStep);
            }

            if (AllowDbNull)
            {
                sql += " NOT NULL ";
            }

            //пока не реализовано
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

    }
}
