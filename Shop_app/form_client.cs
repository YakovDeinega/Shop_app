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
    public partial class form_client : Form
    {
        public NpgsqlConnection con;
        public form_client(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO client (name, address, tel) VALUES (:name, :address, :tel)", con);
                command.Parameters.AddWithValue("name", textBox3.Text);
                command.Parameters.AddWithValue("address", textBox1.Text);
                command.Parameters.AddWithValue("tel", textBox2.Text);
                command.ExecuteNonQuery();
                Close();
            }
            catch
            {

            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
