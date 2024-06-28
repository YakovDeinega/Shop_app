using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static Shop_app.ivoices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Shop_app
{
    public partial class clients : Form
    {
        public NpgsqlConnection con;
        int id;
        List<Int32> idxs = new List<Int32>();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public void Update()
        {
            String sql = "Select * from client";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dg.DataSource = dt;
            dg.Columns[0].HeaderText = "id";
            dg.Columns[1].HeaderText = "ФИО";
            dg.Columns[2].HeaderText = "Телефон";
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public clients(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
            Update();
        }


        private void добавитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            clients_form f = new clients_form(con);
            f.ShowDialog();
            Update();
        }

        private void удалитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string message = "Вы точно хотите удалить?";
            string caption = "Подтверждение операции";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                int id = (int)dg.CurrentRow.Cells["id"].Value;
                NpgsqlCommand command = new NpgsqlCommand("Delete from client where id = :id", con);
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                Update();
            }
        }

        private void dg_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dg.SelectedRows;
            if (selectedRows.Count > 0)
            {
                DataGridViewRow row = selectedRows[0];
                id = Convert.ToInt32(row.Cells[0].Value);
                idxs.Add(id);
                textBox1.Text = Convert.ToString(row.Cells[1].Value);
                textBox2.Text = Convert.ToString(row.Cells[2].Value);
            }
        }


        private void обновитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string sql = string.Format("UPDATE client set fio = '{0}', tel = '{1}' where id = {2}", textBox1.Text, textBox2.Text, id);
            NpgsqlCommand npc = new NpgsqlCommand(sql, con);
            npc.ExecuteNonQuery();
            Update();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime date1 = dt1.Value;
            DateTime date2 = dt2.Value;
            string sql = string.Format("select zakaz_1.id, zakaz_1.date, zakaz_rasch.id_t, zakaz_rasch.sum from client join zakaz_1 on client.id=zakaz_1.id_cl join zakaz_rasch on zakaz_1.id=zakaz_rasch.id_zak where zakaz_rasch.dost=false and client.id = '{0}' and zakaz_1.date between '{1}' and '{2}'", id,date1,date2);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dg.DataSource = dt;
            dg.Columns[0].HeaderText = "id заказа";
            dg.Columns[1].HeaderText = "Дата";
            dg.Columns[2].HeaderText = "id товара";
            dg.Columns[3].HeaderText = "Сумма";
            this.StartPosition = FormStartPosition.CenterScreen;


        }
    }
}
