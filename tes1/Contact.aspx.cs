using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace tes1
{
    public partial class Contact : Page
    {
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            autoNumberKD_Pembayaran();
            {
                if (!this.IsPostBack)
                {
                    BoundField bfield = new BoundField();
                    bfield.HeaderText = "Nama Barang";
                    bfield.DataField = "Nama Barang";
                    detailgrid.Columns.Add(bfield);

                    TemplateField tfield = new TemplateField();
                    tfield.HeaderText = "Qty";
                    detailgrid.Columns.Add(tfield);

                    TemplateField tfield1 = new TemplateField();
                    tfield1.HeaderText = "Harga";
                    detailgrid.Columns.Add(tfield1);

                    TemplateField tfield2 = new TemplateField();
                    tfield2.HeaderText = "Total";
                    detailgrid.Columns.Add(tfield2);
                }

                this.BindGrid();
            }

            isi_dropdown();

        }

        private void BindGrid()
        {

            dt.Columns.Add("Nama Barang");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Harga");
            dt.Columns.Add("Total");
            dt.Rows.Add("elips", 2, 10000, 20000);
            detailgrid.DataSource = dt;
            detailgrid.DataBind();
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtCountry = new TextBox();
                txtCountry.ID = "txtName";
                txtCountry.Text = (e.Row.DataItem as DataRowView).Row["Name"].ToString();
                e.Row.Cells[1].Controls.Add(txtCountry);
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string result = "";
            for (var row = 0; row < detailgrid.Rows.Count; row++)
            {
                TextBox txt2 = detailgrid.Rows[row].FindControl("txtName") as TextBox;
                result = result + ";" + txt2.Text;
            }
            Response.Write(result);

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            tanggaltransaksi.Text = Calendar1.SelectedDate.ToShortDateString() + '.';
        }

        private void autoNumberKD_Pembayaran()
        {
            long hitung;
            string urut;

            String tes = "Server=DESKTOP-PN230RF;Database=DBtest;User Id=sa;Password=123456";
            SqlConnection destinationConnection = new SqlConnection(tes);
            destinationConnection.Open();
            // Perintah untuk mendapatkan nilai terbesar dari field nomor
            SqlCommand cmd = new SqlCommand("select kodeTransaksi from MasterTransaksi where kodeTransaksi in(select max(kodeTransaksi) from MasterTransaksi) order by kodeTransaksi desc", destinationConnection);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            // Jika data ditemukan
            if (dr.HasRows)
            {
                // Menambahkan data dari field nomor
                hitung = Convert.ToInt64(dr[0].ToString().Substring(dr["kodeTransaksi"].ToString().Length - 3, 3)) + 1;
                string joinstr = "00" + hitung;
                // Mengambil 4 karakter kanan terakhir dari string joinstr lalu di tambahkan dengan string URUT
                urut = "KD" + joinstr.Substring(joinstr.Length - 3, 3);
            }
            else
            {
                // Jika tidak ditemukan maka mengisi variable urut dengan YPMB-0001
                urut = "KD001";
            }
            dr.Close();
            kodetransaksi.Text = urut; // Nama textBox nya adalah kdPembayaran.Text
            destinationConnection.Close();
        }

        private void isi_dropdown()
        {
            String tes = "Server=DESKTOP-PN230RF;Database=DBtest;User Id=sa;Password=123456";
            SqlConnection destinationConnection = new SqlConnection(tes);
            destinationConnection.Open();
            String SQL = "SELECT KodeOutlet, NamaOutlet FROM MasterOutlet";
            var dsView = new DataSet();
            var adp = new SqlDataAdapter(SQL, destinationConnection);
            adp.Fill(dsView, "Outlet");

            namaoutlet.DataSource = dsView;
            namaoutlet.DataTextField = "NamaOutlet";
            namaoutlet.DataValueField = "KodeOutlet";
            namaoutlet.DataBind();
            adp.Dispose();
            destinationConnection.Close();
        }

        protected void addButton_Click(object sender, EventArgs e)
        {
            int Total = Convert.ToInt32(Qty.Text) * Convert.ToInt32(Harga.Text);
            dt.Rows.Add(NamaBarang.Text, Qty.Text, Harga.Text , Total);
            detailgrid.DataSource = dt;
            detailgrid.DataBind();
        }
    }
}