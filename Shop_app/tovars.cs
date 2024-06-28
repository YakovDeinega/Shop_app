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

namespace Shop_app
{
    public partial class tovars : Form
    {
        public NpgsqlConnection con;
        int id;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public void Update()
        {
            String sql = "Select * from tovar";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dg.DataSource = dt;
            dg.Columns[0].HeaderText = "id";
            dg.Columns[1].HeaderText = "Название товара";
            dg.Columns[2].HeaderText = "Тип товара";
            this.StartPosition= FormStartPosition.CenterScreen;
        }
        public tovars(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
            Update();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string message = "Вы точно хотите удалить?";
            string caption = "Подтверждение операции";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                int id = (int)dg.CurrentRow.Cells["id"].Value;
                NpgsqlCommand command = new NpgsqlCommand("Delete from tovar where id = :id", con);
                NpgsqlCommand com = new NpgsqlCommand("Delete from price_list where id_t=:id", con);
                command.Parameters.AddWithValue("id", id);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
                command.ExecuteNonQuery();
                Update();
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tovars_form f = new tovars_form(con);
            f.ShowDialog();
            Update();
        }

        private void dg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dg.SelectedRows;
            if (selectedRows.Count > 0)
            {
                DataGridViewRow row = selectedRows[0];
                id = Convert.ToInt32(row.Cells[0].Value);
                textBox1.Text = Convert.ToString(row.Cells[1].Value);
                textBox2.Text = Convert.ToString(row.Cells[2].Value);
            }
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = string.Format("UPDATE tovar set name = '{0}', type = '{1}' where id = {2}", textBox1.Text, textBox2.Text, id);
            NpgsqlCommand npc = new NpgsqlCommand(sql, con);
            npc.ExecuteNonQuery();
            Update();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
