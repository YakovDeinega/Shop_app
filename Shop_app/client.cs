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
    public partial class client : Form
    {
        public NpgsqlConnection con;
        int id;
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
            dg.Columns[0].HeaderText = "Номер";
            dg.Columns[1].HeaderText = "Имя";
            dg.Columns[2].HeaderText = "Адрес";
            dg.Columns[3].HeaderText = "Телефон";
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public client(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
            Update();
        }


        private void добавитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            form_client f = new form_client(con);
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
                int id = (int)dg.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand command = new NpgsqlCommand("Delete from client where ID = :id", con);
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
                textBox1.Text = Convert.ToString(row.Cells[1].Value);
                textBox2.Text = Convert.ToString(row.Cells[2].Value);
                textBox3.Text = Convert.ToString(row.Cells[3].Value);
            }
        }


        private void обновитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string sql = string.Format("UPDATE client set name = '{0}', address = '{1}', tel = '{2}' where id = {3}", textBox1.Text, textBox2.Text, textBox3.Text, id);
            NpgsqlCommand npc = new NpgsqlCommand(sql, con);
            npc.ExecuteNonQuery();
            Update();
        }
    }
}
