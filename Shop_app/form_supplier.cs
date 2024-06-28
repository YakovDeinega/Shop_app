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
    public partial class form_supplier : Form
    {
        public NpgsqlConnection con;
        public form_supplier(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO suppliers (supplier_name, contact) VALUES (:name, :con)", con);
            command.Parameters.AddWithValue("name", textBox3.Text);
            command.Parameters.AddWithValue("con", textBox1.Text);
            command.ExecuteNonQuery();
            Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
