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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Shop_app
{
    public partial class ivoices : Form
    {
        public NpgsqlConnection con;
        int id;
        int id_client;
        int id_tovar;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public void Update()
        {
            String sql = "Select * from zakaz_1";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dg.DataSource = dt;
            dg.Columns[0].HeaderText = "id";
            dg.Columns[1].HeaderText = "id клиента";
            dg.Columns[2].HeaderText = "Дата";
            dg.Columns[3].HeaderText = "Адрес";
            dg.Columns[4].HeaderText = "Полная сумма";
            dg.Columns[5].HeaderText = "Доставленная сумма";
            this.StartPosition = FormStartPosition.CenterScreen;
            List <Client> list = new List <Client>();
            string request = "select id, fio from client";
            NpgsqlCommand com = new NpgsqlCommand(request, con);
            using (NpgsqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    string str = reader["id"].ToString();
                    int id_cl = Convert.ToInt32(str);
                    string fio = reader["fio"].ToString();
                    Client client = new Client(id_cl, fio);
                    list.Add(client);
                }
            }
            comboBox1.DataSource = list;
            comboBox1.DisplayMember = "fio";
            comboBox1.ValueMember = "id";
        }
        public ivoices(NpgsqlConnection con)
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
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO zakaz_1 (id_cl, date, address, total_sum, dost_sum) VALUES (:id_cl, :date, :address, :total_sum, :dost_sum)", con);
            command.Parameters.AddWithValue("id_cl", id_client);
            command.Parameters.AddWithValue("date", dt1.Value);
            command.Parameters.AddWithValue("address", addresstx.Text);
            command.Parameters.AddWithValue("total_sum", 0);
            command.Parameters.AddWithValue("dost_sum", 0);
            command.ExecuteNonQuery();
            Update();
        }

        private void обновитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string sql = string.Format("UPDATE zakaz_1 set date = '{0}', address='{1}' where id = {2}", dt1.Value, addresstx.Text, id) ;
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
                int id = (int)dg.CurrentRow.Cells["id"].Value;
                NpgsqlCommand command = new NpgsqlCommand("Delete from zakaz_1 where id = :id", con);
                NpgsqlCommand com = new NpgsqlCommand("Delete from zakaz_rasch where id_zak = :id", con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
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
            public int id { get; set; }
            public string fio { get; set; }
            public Client(int id, string fio)
                { this.id = id; this.fio = fio; }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Client client = (Client)comboBox1.SelectedItem;
            id_client = client.id;
        }

        private void dg_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        public class Tovar
        {
            public int id { get; set; }
            public string name { get; set; }
            public Tovar(int id, string name)
            { this.id = id; this.name = name; }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tovar tovar = (Tovar)comboBox2.SelectedItem;
            id_tovar = tovar.id;
        }


        DataTable dt2 = new DataTable();
        DataSet ds1 = new DataSet();
        public void Update1()
        {
            String sql = "Select * from zakaz_rasch";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            ds1.Reset();
            da1.Fill(ds1);
            dt2 = ds1.Tables[0];
            dg2.DataSource = dt2;
            dg2.Columns[0].HeaderText = "id_str";
            dg2.Columns[1].HeaderText = "id заказа";
            dg2.Columns[2].HeaderText = "id товара";
            dg2.Columns[3].HeaderText = "количество";
            dg2.Columns[4].HeaderText = "сумма";
            dg2.Columns[5].HeaderText = "статус доставки";
            this.StartPosition = FormStartPosition.CenterScreen;
            List<Tovar> list = new List<Tovar>();
            string request = "select id as id_tov, name from tovar";
            NpgsqlCommand com = new NpgsqlCommand(request, con);
            using (NpgsqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    string str = reader["id_tov"].ToString();
                    int id_t = Convert.ToInt32(str);
                    string name = reader["name"].ToString();
                    Tovar tovar = new Tovar(id_t, name);
                    list.Add(tovar);
                }
            }
            comboBox2.DataSource = list;
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cost = "1";
            NpgsqlCommand com = new NpgsqlCommand("select cost from price_list where id_t = :id_tovar", con);
            com.Parameters.AddWithValue("id_tovar", id_tovar);
            using (NpgsqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    cost = reader["cost"].ToString();
                }
            }
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO zakaz_rasch (id_zak, id_t, kol, sum, dost) VALUES (:id_zak, :id_t, :kol, :sum, :dost)", con);
            command.Parameters.AddWithValue("id_zak", id);
            command.Parameters.AddWithValue("id_t", id_tovar);
            command.Parameters.AddWithValue("kol", Convert.ToInt32(kolvotx.Text));
            command.Parameters.AddWithValue("sum", Convert.ToInt32(kolvotx.Text)*Convert.ToInt32(cost));
            command.Parameters.AddWithValue("dost", false);
            command.ExecuteNonQuery();
            Update1();
            Update();
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
            String sql = "Select * from zakaz_rasch where id_zak="+id;
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            da1.Fill(dt);
            DataColumnCollection col = dt.Columns;
            col[0].ColumnName = "id_str";
            col[1].ColumnName = "id заказа";
            col[2].ColumnName = "id товара";
            col[3].ColumnName = "количество";
            col[4].ColumnName = "сумма";
            col[5].ColumnName = "статус доставки";
            DataView dv = new DataView(dt);
           // dv.RowFilter = "Номер накладной =" + id;
            dg2.DataSource = dv;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string message = "Вы точно хотите удалить?";
            string caption = "Подтверждение операции";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                int id = (int)dg2.CurrentRow.Cells["id_str"].Value;
                NpgsqlCommand command = new NpgsqlCommand("Delete from zakaz_rasch where id = :id", con);
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                Update1();
                Update();
            }
        }

        private void обновитьСтатусДоставкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void dg2_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dg2.SelectedRows;
            int kol = 0;
            int id_tovara = 0;
            int kol_sklada = 0;
            bool dostavka = false;
            if (selectedRows.Count > 0)
            {
                DataGridViewRow row = selectedRows[0];
                id_tovara= Convert.ToInt32(row.Cells[2].Value);
                kol = Convert.ToInt32(row.Cells[3].Value);
                dostavka = Convert.ToBoolean(row.Cells[5].Value);
                string request = "select kol from sklad where id_t=:id";
                NpgsqlCommand com = new NpgsqlCommand(request, con);
                com.Parameters.AddWithValue("id", id_tovara);
                using (NpgsqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        kol_sklada = Convert.ToInt32(reader["kol"].ToString());
                    }
                }
                if (kol <= kol_sklada && dostavka==false)
                {
                    kol_sklada -= kol;
                    string sql = string.Format("UPDATE sklad set kol = '{0}' where id_t = '{1}'", kol_sklada, id_tovara);
                    NpgsqlCommand npc = new NpgsqlCommand(sql, con);
                    npc.ExecuteNonQuery();
                    string sql2 = string.Format("UPDATE zakaz_rasch set dost = '{0}' where id_t = '{1}'", true, id_tovara);
                    NpgsqlCommand npc2 = new NpgsqlCommand(sql2, con);
                    npc2.ExecuteNonQuery();
                    Update();
                    Update1();
                }

            }
        }
    }
}
