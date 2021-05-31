using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeRDLC
{
    public partial class Form1 : Form
    {
        private string savePath = @"C:\Users\SHARMILA\Desktop\Project";

        public Form1()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;
            reportViewer1.LocalReport.ReportPath = "EmployeeReport.rdlc";
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeeWarehouse;Integrated Security=True;");
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.EmpTab", con);
            EmpDataSet empDataSet = new EmpDataSet();
            da.Fill(empDataSet, "EmpTab");
            con.Close();
            ReportDataSource rds = new ReportDataSource("EmpDataSet", empDataSet.Tables[0]);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
            SavePdf(reportViewer1);


            /*this.reportViewer1.LocalReport.ReportPath = "EmployeeServerRDLC.rdlc";
            ReportDataSource rds = new ReportDataSource("EmployeeServerRDLC", GetEmployees());
            this.reportViewer1.LocalReport.DataSources.Add(rds);*/
        }

        private void SavePdf(ReportViewer reportViewer1)
        {
            SaveFileDialog saveFileDialogPDF = new SaveFileDialog();
            saveFileDialogPDF.Filter = "PDF|*.pdf";
            saveFileDialogPDF.Title = "Save report to PDF";
            saveFileDialogPDF.ShowDialog();
            if (saveFileDialogPDF.FileName != "")
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;
                byte[] bytes = reportViewer1.LocalReport.Render(
                           "PDF", null, out mimeType, out encoding, out filenameExtension,
                           out streamids, out warnings);
                using (FileStream fs = new FileStream(saveFileDialogPDF.FileName, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }
        private List<Employee> GetEmployees()
        {
            return new List<Employee>
            {
                new Employee{Id=1,F_Name="Janhavi",L_Name="Prte",Address="Mumbai 42",City="Mumbai",Contact_no="97542"},
                new Employee{Id=2,F_Name="Aavi",L_Name="Prte",Address="Delhi 42",City="Delhi",Contact_no="97712"}

            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private DataSet GetData()
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeeWarehouse;Integrated Security=True;");
            con.Open();
           // SqlCommand cmd = new SqlCommand("select * from dbo.EmpTab", con);
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.EmpTab",con);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            con.Close();
            return null;
        }
    }
}
