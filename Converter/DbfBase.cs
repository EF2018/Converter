using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace Converter
{
    public class DbfBase : Base<DbfField>
    {
        public DbfBase(string basename, string dirPath = "C:\\Users\\Admin.Admin-ПК\\Desktop")
        {
            BaseName = basename;
            DirPath = dirPath + "\\" + BaseName;
            Conn = new OdbcConnection(@"Dsn=dBASE Files;Driver={""Microsoft dBase Driver(*.DBF)""};dbq=" + DirPath + ";defaultdir=" + DirPath + ";driverid=533;maxbuffersize=2048;pagetimeout=5");
            Cmd = Conn.CreateCommand();
            MyDataSet = new DataSet();
        }

        // Создание таблицы файла базы данных
        public override void Create()//List <DbfField> structure, string nameTbl)
        {
            DirectoryInfo dir = new DirectoryInfo(DirPath);
            dir.Create();
            MyDataSet.WriteXmlSchema(DirPath + "\\" + BaseName + "_sheme.xsd");//запись схемы данных в Xml
            try
            {
                Conn.Open();
                if (MyDataSet != null)
                {
                    foreach (DataTable tbl in MyDataSet.Tables)
                    {
                        Cmd.CommandText = string.Format(@"CREATE TABLE [{0}] (", tbl.TableName);

                        foreach (DataColumn col in tbl.Columns)
                        {
                            DbfField field = new DbfField(col);
                            Cmd.CommandText += string.Format(@"{0} {1}, ", col.ColumnName, field.GenerateSql());
                        }
                        Cmd.CommandText = Cmd.CommandText.Remove(Cmd.CommandText.Length - 2);
                        Cmd.CommandText += string.Format(@")");
                        Cmd.ExecuteNonQuery();
                        MessageBox.Show("DataBase (dbf): " + BaseName + " has created Successfully", "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        //Положить данные в DataSet
        public override void ImportDataToDataSet()
        {
            DirectoryInfo dir = new DirectoryInfo(DirPath);
            //плучаем все dbf файлы в директории
            FileInfo[] dbffileDir = dir.GetFiles("*.dbf", SearchOption.AllDirectories);

            foreach (FileInfo dbffile in dbffileDir)
            {
                DataTable tblfromfile = GetAllRecords(dbffile.Name);
                if (MyDataSet.Tables.Contains(dbffile.Name) || MyDataSet.Tables[dbffile.Name].Equals(tblfromfile))
                {
                    foreach (DataRow row in tblfromfile.Rows)
                    {
                        MyDataSet.Tables[dbffile.Name].Rows.Add(row);
                    };
                }
                MyDataSet.Tables.Add(tblfromfile);
            }
        }

        public override void ExportDataToBase()
        {
            //в работе
        }
    }
}

