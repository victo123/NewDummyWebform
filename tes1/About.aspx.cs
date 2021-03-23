using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExcelDataReader;

namespace tes1
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                try
                {
                    var filename = Path.GetFileName(FileUploadControl.FileName);
                    StatusLabel.Text = Server.MapPath("~/") + filename;
                    FileStream stream = File.Open(Server.MapPath("~/") + filename, FileMode.Open, FileAccess.Read);
                    var drexcel = readFileExcel(stream);

                    var dtWriteoffUpload = new DataTable();
                    dtWriteoffUpload.Columns.Add("kodeoutlet");
                    dtWriteoffUpload.Columns.Add("namaoutlet");
                    dtWriteoffUpload.Columns.Add("alamat");
                    dtWriteoffUpload.Columns.Add("outletkota");
                    dtWriteoffUpload.Columns.Add("groupcode");
                    dtWriteoffUpload.Columns.Add("subgroupcode");
                    dtWriteoffUpload.Columns.Add("classcode");
                    dtWriteoffUpload.Columns.Add("namagroup2");

                    while (drexcel.Read())
                    {
                        dtWriteoffUpload.Rows.Add(drexcel.GetValue(0), drexcel.GetValue(1), drexcel.GetValue(2), drexcel.GetValue(10), drexcel.GetValue(11), drexcel.GetValue(12), drexcel.GetValue(13), drexcel.GetValue(14));
                    }

                    //Validasi(dtWriteoffUpload);

                    dtWriteoffUpload.Rows.RemoveAt(0); //  delete header

                    InsertTableIntoDB(dtWriteoffUpload);

                    StatusLabel.Text = "Upload status: File uploaded!";
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }

        public void Validasi(System.Data.DataTable tblFormat)
        {
            String tes = "Server=DESKTOP-PN230RF;Database=DBtest;User Id=sa;Password=123456";
            for (int i = 0; i < tblFormat.Rows.Count; i++)
            {
                String groupcode = tblFormat.Rows[i]["groupcode"].ToString();
                String sql1 = "SELECT dintinct id from Group where id=" + groupcode;
                SqlConnection con1 = new SqlConnection(tes);
                var dbcm1 = new SqlCommand(sql1, con1);
                using (SqlDataReader data = dbcm1.ExecuteReader())
                {
                    if (data.Read() == false)
                    {
                        throw new ArgumentException("groupcode not registered " + groupcode);
                    }
                }

                String subgroupcode = tblFormat.Rows[i]["subgroupcode"].ToString();
                String sql2 = "SELECT dintinct id from Group where id=" + subgroupcode;
                SqlConnection con2 = new SqlConnection(tes);
                var dbcm2 = new SqlCommand(sql2, con2);
                using (SqlDataReader data = dbcm2.ExecuteReader())
                {
                    if (data.Read() == false)
                    {
                        throw new ArgumentException("subgroupcode not registered " + subgroupcode);
                    }
                }

                String classcode = tblFormat.Rows[i]["classcode"].ToString();
                String sql3 = "SELECT dintinct id from Group where id=" + classcode;
                SqlConnection con3 = new SqlConnection(tes);
                var dbcm3 = new SqlCommand(sql3, con3);
                using (SqlDataReader data = dbcm3.ExecuteReader())
                {
                    if (data.Read() == false)
                    {
                        throw new ArgumentException("classcode not registered " + classcode);
                    }
                }

                String namagroup2 = tblFormat.Rows[i]["namagroup2"].ToString();
                String sql4 = "SELECT dintinct id from Group where id=" + classcode;
                SqlConnection con4 = new SqlConnection(tes);
                var dbcm4 = new SqlCommand(sql4, con4);
                using (SqlDataReader data = dbcm4.ExecuteReader())
                {
                    if (data.Read() == false)
                    {
                        throw new ArgumentException("classcode not registered " + classcode);
                    }
                }
            }
        }

        public void InsertTableIntoDB(System.Data.DataTable tblFormat)
        {
            for (int i = 0; i < tblFormat.Rows.Count; i++)
            {

                String InsertQuery = string.Empty;

                InsertQuery = "INSERT INTO MasterOutlet " +
                              "(KodeOutlet,NamaOutlet,Alamat,OutletKota,GroupCode,SubGroupCode,ClassCode,NamaGroup2) " +
                              "VALUES ('"
                              + tblFormat.Rows[i]["kodeoutlet"].ToString() + "','"
                              + tblFormat.Rows[i]["namaoutlet"].ToString()
                              + "','" + tblFormat.Rows[i]["alamat"].ToString()
                              + "','" + tblFormat.Rows[i]["outletkota"].ToString()
                              + "','" + tblFormat.Rows[i]["groupcode"].ToString()
                              + "','" + tblFormat.Rows[i]["subgroupcode"].ToString()
                              + "','" + tblFormat.Rows[i]["classcode"].ToString()
                              + "','" + tblFormat.Rows[i]["namagroup2"].ToString() + "')";

                //using (SqlConnection destinationConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
                String tes = "Server=DESKTOP-PN230RF;Database=DBtest;User Id=sa;Password=123456";
                using (SqlConnection destinationConnection = new SqlConnection(tes))
                using (var dbcm = new SqlCommand(InsertQuery, destinationConnection))
                {
                    destinationConnection.Open();
                    dbcm.ExecuteNonQuery();
                }
            }
        }


        protected IExcelDataReader readFileExcel(Stream Stremfile)
        {
            var reader = ExcelReaderFactory.CreateReader(Stremfile, new ExcelReaderConfiguration()
            {
                // Gets or sets the encoding to use when the input XLS lacks a CodePage
                // record, or when the input CSV lacks a BOM and does not parse as UTF8. 
                // Default: cp1252 (XLS BIFF2-5 and CSV only)
                FallbackEncoding = Encoding.GetEncoding(1252),

                // Gets or sets the password used to open password protected workbooks.
                //Password = "password",

                // Gets or sets an array of CSV separator candidates. The reader 
                // autodetects which best fits the input data. Default: , ; TAB | # 
                // (CSV only)
                AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },

                // Gets or sets a value indicating whether to leave the stream open after
                // the IExcelDataReader object is disposed. Default: false
                LeaveOpen = false,

                // Gets or sets a value indicating the number of rows to analyze for
                // encoding, separator and field count in a CSV. When set, this option
                // causes the IExcelDataReader.RowCount property to throw an exception.
                // Default: 0 - analyzes the entire file (CSV only, has no effect on other
                // formats)
                AnalyzeInitialCsvRows = 0,
            });

            return reader;
        }
    }
}