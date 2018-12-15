using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.Linq;
using System.Reflection;

namespace Converter
{
    public abstract class Base<T> : IFormat
    {
        public string DirPath { get; set; }
        protected DbCommand Cmd { get; set; }
        public DbConnection Conn { get; set; }
        public DataSet MyDataSet { get; set; }
        public string BaseName { get; set; }


        public void InitExistingBase()
        {
            MyDataSet.ReadXml(DirPath + "\\" + BaseName + "_sheme.xsd");
        }

        public virtual void Create() { }

        //Конвертация
        public virtual IFormat ConvertTo(IFormat newformatfile)
        {
            newformatfile.MyDataSet = MyDataSet;
            return newformatfile;
        }

        //Импорт данных из БД в DataSet
        public virtual void ImportDataToDataSet()
        {
        }

        //Экспорт данных из DataSet в БД 
        public virtual void ExportDataToBase()
        {

        }

        public virtual DataTable GetAllRecords(string tblName)
        {
            DataTable dt = null;
            if (Conn != null)
            {
                try
                {
                    Conn.Open();
                    dt = new DataTable();
                    Cmd.CommandText = String.Format("Use {0} SELECT * FROM {1}", BaseName, tblName);
                    dt.Load(Cmd.ExecuteReader());
                    Conn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            return dt;
        }


        public virtual void Execute(string command)
        {
            if (Conn != null)
            {
                try
                {
                    Conn.Open();
                    Cmd.CommandText = command;
                    Cmd.ExecuteNonQuery();
                    Conn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}
