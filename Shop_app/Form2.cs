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
    public partial class Form2 : Form
    {
        public NpgsqlConnection con;
        public Form2(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO tovars (tovar_title, description, unit) VALUES (:title, :desc, :unit)", con);
            command.Parameters.AddWithValue("title", textBox1.Text);
            command.Parameters.AddWithValue("desc", textBox2.Text);
            command.Parameters.AddWithValue("unit", textBox3.Text);
            command.ExecuteNonQuery();
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
    }
}
