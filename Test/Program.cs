using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Converter;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DbfBase dbffile = new DbfBase("DbfBase");

            //---Создаем таблицу компаний
            DataTable tblCompanies = new DataTable("Companies");
            DataColumn col1 = new DataColumn("IdCompany", typeof(int))
            {
                Caption = "IdCompany",
                //AllowDBNull = false,
                //AutoIncrement = true,
                //AutoIncrementSeed = 0,
                //AutoIncrementStep = 1,
                Unique = true
            };
            tblCompanies.Columns.Add(col1);

            DataColumn col2 = new DataColumn("NameCompany", typeof(string))
            {
                Caption = "NameCompany",
                //AllowDBNull = true,
                MaxLength = 50,
            };
            tblCompanies.Columns.Add(col2);

            DataColumn col3 = new DataColumn("IdCity", typeof(int))
            {
                Caption = "IdCity",
                AllowDBNull = true
            };
            tblCompanies.Columns.Add(col3);

            //-----------------Создаем таблицу контактов
            DataTable tblContacts = new DataTable("Contacts");
            DataColumn col4 = new DataColumn("IdCompany", typeof(int))
            {
                Caption = "IdCompany",
                AllowDBNull = false
            };
            tblContacts.Columns.Add(col4);

            DataColumn col5 = new DataColumn("IdContact", typeof(int))
            {
                Caption = "IdContact",
                AllowDBNull = false,
                //AutoIncrementSeed = 234,
                //AutoIncrement = true,
                //AutoIncrementStep = 1,
                Unique = true
            };
            tblContacts.Columns.Add(col5);

            DataColumn col6 = new DataColumn("NameContact", typeof(string))
            {
                Caption = "NameContact",
                AllowDBNull = true,
                MaxLength = 100
            };
            tblContacts.Columns.Add(col6);

            DataColumn col7 = new DataColumn("Phone", typeof(string))
            {
                Caption = "Phone",
                AllowDBNull = true,
                MaxLength = 100
            };
            tblContacts.Columns.Add(col7);

            dbffile.MyDataSet.Tables.Add(tblCompanies);
            dbffile.MyDataSet.Tables.Add(tblContacts);

            //Создаем связь таблиц
            //dbffile.MyDataSet.Tables["Contacts"].Constraints.Add(new ForeignKeyConstraint(dbffile.MyDataSet.Tables["Company"].Columns["IdCompany"],
            //   dbffile.MyDataSet.Tables["Contacts"].Columns["IdCompany"]));

            //dbffile.MyDataSet.Tables["Company"].PrimaryKey = new DataColumn[1] { dbffile.MyDataSet.Tables["Contacts"].Columns["IdCompany"] };

            //dbffile.MyDataSet.Relations.Add("CompanyContact", dbffile.MyDataSet.Tables["Company"].Columns["IdCompany"], dbffile.MyDataSet.Tables["Contacts"].Columns["IdContact"]);
            dbffile.Create();

            SqlBase SqlBase = (SqlBase)dbffile.ConvertTo(new SqlBase("SqlBase"));
            SqlBase.Create();

            SqlBase SqlBase1 = new SqlBase("SqlBase");
            SqlBase1.InitExistingBase();

            DataTable tblnew = SqlBase1.MyDataSet.Tables["Companies"];
            DataRow newrow = tblnew.NewRow();
            newrow[0] = 1;
            newrow[1] = "Company1";
            newrow[2] = 1;
            SqlBase1.MyDataSet.Tables["Companies"].Rows.Add(newrow);

            DataRow newrow2 = tblnew.NewRow();
            newrow2[0] = 2;
            newrow2[1] = "Company2";
            newrow2[2] = 2;
            SqlBase1.MyDataSet.Tables["Companies"].Rows.Add(newrow2);

            DataRow newrow3 = tblnew.NewRow();
            newrow3[0] = 3;
            newrow3[1] = "Company3";
            newrow3[2] = 3;
            SqlBase1.MyDataSet.Tables["Companies"].Rows.Add(newrow3);

            DataTable tblnew1 = SqlBase1.MyDataSet.Tables["Contacts"];
            DataRow newrow1 = tblnew1.NewRow();
            newrow1[0] = 1;
            newrow1[1] = 1;
            newrow1[2] = "Contact1";
            newrow1[3] = "1111111";
            SqlBase1.MyDataSet.Tables["Contacts"].Rows.Add(newrow1);

            DataRow newrow4 = tblnew1.NewRow();
            newrow4[0] = 2;
            newrow4[1] = 2;
            newrow4[2] = "Contact2";
            newrow4[3] = "222222";
            SqlBase1.MyDataSet.Tables["Contacts"].Rows.Add(newrow4);

            SqlBase1.ExportDataToBase();

            SqlBase sqlBase2 = new SqlBase("SqlBase");
            sqlBase2.InitExistingBase();
            sqlBase2.ImportDataToDataSet();
            //DbfFile d = new DbfFile("d");
            //var d = nn.ConvertTo(new DbfFile(nn.FileName));
        }
    }


}
