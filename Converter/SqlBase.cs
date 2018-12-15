using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.Linq;

namespace Converter
{
    public class SqlBase : Base<SqlField>
    {
        public SqlBase(string basename, string catalog = "master", string datasource = "ADMIN-ПК\\SQLEXPRESSEUG", string dirPathXsd = "C:\\Users\\Admin.Admin-ПК\\Desktop")
        {
            BaseName = basename;
            DirPath = dirPathXsd;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = datasource,
                InitialCatalog = catalog,
                IntegratedSecurity = true,
            };
            Conn = new SqlConnection();
            Conn.ConnectionString = builder.ConnectionString;
            Cmd = Conn.CreateCommand();
            MyDataSet = new DataSet();
        }


        public override void Create()
        {
            MyDataSet.WriteXmlSchema(DirPath + "\\" + BaseName + "_sheme.xsd");// запись схемы данных            
            try
            {
                Conn.Open();

                if (MyDataSet != null)
                {
                    Cmd.CommandText = string.Format("if (db_id('{0}') is not null) DROP DATABASE [{0}] CREATE DATABASE [{0}] ", BaseName);
                    Cmd.ExecuteNonQuery();

                    foreach (DataTable tbl in MyDataSet.Tables)
                    {
                        Cmd.CommandText = string.Format("USE [{0}] CREATE TABLE {1} (", BaseName, tbl.TableName);

                        foreach (DataColumn col in tbl.Columns)
                        {
                            Cmd.CommandText += string.Format("[{0}] {1}, ", col.ColumnName, new SqlField(col).GenerateSql());
                        }

                        Cmd.CommandText = Cmd.CommandText.Remove(Cmd.CommandText.Length - 2);
                        Cmd.CommandText += string.Format(@")");
                        Cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("DataBase (sql): " + BaseName + " has created Successfully", "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public override void ExportDataToBase()
        {
            foreach (DataTable tbl in MyDataSet.Tables)
            {
                string StrCommand = String.Format("USE {0}", BaseName);
                foreach (DataRow row in tbl.Rows)
                {
                    StrCommand += string.Format(" INSERT INTO {0} VALUES (", tbl.TableName);
                    for (int i = 0; i < tbl.Columns.Count; i++)
                    {
                        if ((tbl.Columns.Count - i) != 1)
                        {
                            StrCommand += string.Format("'{0}', ", row[i]);
                        }
                        else
                        {
                            StrCommand += string.Format("'{0}')", row[i]);
                        }
                    }
                }
                Execute(StrCommand);
            }
        }

        public override void ImportDataToDataSet()
        {
            MyDataSet.WriteXmlSchema(DirPath + "\\" + BaseName + "_sheme.xsd");// запись схемы данных            
            foreach (DataTable tbl in MyDataSet.Tables)
            {
                DataTable tblfromfile = GetAllRecords(tbl.TableName);
                foreach (DataRow row in tblfromfile.Rows)
                {
                    MyDataSet.Tables[tbl.TableName].Rows.Add(row);
                };
            }
        }


    }
}
