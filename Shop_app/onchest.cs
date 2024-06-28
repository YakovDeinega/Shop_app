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
    public partial class onchest : Form
    {
        public NpgsqlConnection con;
        int id;
        int id_client;
        int id_tovar;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public void Update()
        {
            String sql = "Select * from onchest";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dg.DataSource = dt;
            dg.Columns[0].HeaderText = "Номер";
            dg.Columns[1].HeaderText = "Дата";
            dg.Columns[2].HeaderText = "Имя клиента";
            dg.Columns[3].HeaderText = "Суммарная стоимость";
            this.StartPosition = FormStartPosition.CenterScreen;
            List <Client> list = new List <Client>();
            string request = "select id, name from client";
            NpgsqlCommand com = new NpgsqlCommand(request, con);
            using (NpgsqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    string str = reader["id"].ToString();
                    int id_c = Convert.ToInt32(str);
                    string name = reader["name"].ToString();
                    Client client = new Client(id_c, name);
                    list.Add(client);
                }
            }
            comboBox1.DataSource = list;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "id";
        }
        public onchest(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
            Update();
            Update1();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void добавитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO onchest (data,id_client,summary_cost) VALUES (:data, :id_client, :summary_cost)", con);
            command.Parameters.AddWithValue("data", dt1.Value);
            command.Parameters.AddWithValue("id_client", id_client);
            command.Parameters.AddWithValue("summary_cost", Convert.ToInt32(textBox3.Text));
            command.ExecuteNonQuery();
            Update();
        }

        private void обновитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string sql = string.Format("UPDATE onchest set data = '{0}', id_client = '{1}', summary_cost = '{2}' where id = {3}", dt1.Value, id_client, Convert.ToInt32(textBox3.Text), id) ;
            NpgsqlCommand npc = new NpgsqlCommand(sql, con);
            npc.ExecuteNonQuery();
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
                NpgsqlCommand command = new NpgsqlCommand("Delete from onchest where ID = :id", con);
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                Update();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        public class Client
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Client(int id, string name)
                { this.Id = id; this.Name = name; }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Client client = (Client)comboBox1.SelectedItem;
            id_client = client.Id;
        }

        private void dg_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        public class Tovar
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Tovar(int id, string name)
            { this.Id = id; this.Name = name; }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tovar tovar = (Tovar)comboBox2.SelectedItem;
            id_tovar = tovar.Id;
        }


        DataTable dt2 = new DataTable();
        DataSet ds1 = new DataSet();
        public void Update1()
        {
            String sql = "Select * from rasch";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            ds1.Reset();
            da1.Fill(ds1);
            dt2 = ds1.Tables[0];
            dg2.DataSource = dt2;
            dg2.Columns[0].HeaderText = "Номер товара";
            dg2.Columns[1].HeaderText = "Номер накладной";
            dg2.Columns[2].HeaderText = "Цена";
            dg2.Columns[3].HeaderText = "Количество";
            this.StartPosition = FormStartPosition.CenterScreen;
            List<Tovar> list = new List<Tovar>();
            string request = "select id, name from tovar";
            NpgsqlCommand com = new NpgsqlCommand(request, con);
            using (NpgsqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    string str = reader["id"].ToString();
                    int id_c = Convert.ToInt32(str);
                    string name = reader["name"].ToString();
                    Tovar tovar = new Tovar(id_c, name);
                    list.Add(tovar);
                }
            }
            comboBox2.DataSource = list;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO rasch (id_tovar,id_nakl,cost, kolvo) VALUES (:id_tovar, :id_nakl, :cost, :kolvo)", con);
            command.Parameters.AddWithValue("cost", Convert.ToInt32(sumtx.Text));
            command.Parameters.AddWithValue("kolvo", Convert.ToInt32(kolvotx.Text));
            command.Parameters.AddWithValue("id_tovar", id_tovar);
            command.Parameters.AddWithValue("id_nakl", id);
            command.ExecuteNonQuery();
            Update1();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dg_SelectionChanged_1(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dg.SelectedRows;
            if (selectedRows.Count > 0)
            {
                DataGridViewRow row = selectedRows[0];
                id = Convert.ToInt32(row.Cells[0].Value);
                ViewTovar(id);
            }

        }

        public void ViewTovar(int id)
        {
            String sql = "Select * from rasch where id_nakl="+id;
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            da1.Fill(dt);
            DataColumnCollection col = dt.Columns;
            col[0].ColumnName = "Номер товара";
            col[1].ColumnName = "Номер накладной";
            col[2].ColumnName = "Цена";
            col[3].ColumnName = "Количество";
            DataView dv = new DataView(dt);
           // dv.RowFilter = "Номер накладной =" + id;
            dg2.DataSource = dv;
        }
    }
}
