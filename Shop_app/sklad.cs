using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Shop_app
{
    public partial class sklad : Form
    {
        public NpgsqlConnection con;
        int id;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public void Update()
        {
            String sql = "Select * from sklad";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dg.DataSource = dt;
            dg.Columns[0].HeaderText = "id";
            dg.Columns[1].HeaderText = "Остаток";
            this.StartPosition = FormStartPosition.CenterScreen;
            List<Tovar> list = new List<Tovar>();
            string request = "select id, name from tovar";
            NpgsqlCommand com = new NpgsqlCommand(request, con);
            using (NpgsqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    string str = reader["id"].ToString();
                    int id = Convert.ToInt32(str);
                    string name = reader["name"].ToString();
                    Tovar tovar = new Tovar(id, name);
                    list.Add(tovar);
                }
            }

        }
        public sklad(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
            Update();
        }
        public class Tovar
        {
            public int id { get; set; }
            public string name { get; set; }
            public Tovar(int id, string name)
            { this.id = id; this.name = name; }

        }

        private void sklad_Load(object sender, EventArgs e)
        {

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
                textBox2.Text = Convert.ToString(row.Cells[1].Value);
                List<Tovar> list = new List<Tovar>();
                string request = "select name from tovar where id=:id";
                NpgsqlCommand com = new NpgsqlCommand(request, con);
                com.Parameters.AddWithValue("id", id);
                using (NpgsqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        textBox1.Text = reader["name"].ToString();
                    }
                }

            }
        }

        private void обновитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DateTime myDateTime = DateTime.Now;
            string sql = string.Format("UPDATE sklad set kol = '{0}', date = '{1}' where id_t = '{2}'", Convert.ToInt32(textBox2.Text), myDateTime, id);
            NpgsqlCommand npc = new NpgsqlCommand(sql, con);
            npc.ExecuteNonQuery();
            Update();
        }
    }
}
