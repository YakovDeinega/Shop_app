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
    public partial class tovars_form : Form
    {
        public NpgsqlConnection con;
        public tovars_form(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO tovar (name, type) VALUES (:name, :type) RETURNING id", con);
            command.Parameters.AddWithValue("name", textBox1.Text);
            command.Parameters.AddWithValue("type", textBox2.Text);
            object id = command.ExecuteScalar();
            NpgsqlCommand com = new NpgsqlCommand("INSERT INTO price_list (id_t, cost) VALUES (:id, :cost)", con);
            com.Parameters.AddWithValue("id", id);
            com.Parameters.AddWithValue("cost", 0);
            com.ExecuteNonQuery();
            com = new NpgsqlCommand("INSERT INTO sklad (id_t, kol, date) VALUES (:id, :kol, :date)", con);
            com.Parameters.AddWithValue("id", id);
            com.Parameters.AddWithValue("kol", 0);
            DateTime myDateTime = DateTime.Now;
            
            com.Parameters.AddWithValue("date", myDateTime);
            com.ExecuteNonQuery();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tovars_form_Load(object sender, EventArgs e)
        {

        }
    }
}
