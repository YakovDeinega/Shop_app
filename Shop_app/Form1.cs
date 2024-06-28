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
    public partial class Form1 : Form
    {
        public NpgsqlConnection con;
        public void MyLoad()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            con = new NpgsqlConnection("Server=localhost; Port = 5432; UserID=postgres; Password = 271207; Database = shop");
            con.Open();
        }
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            tovars fp = new tovars(con);
            fp.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            suppliers fp = new suppliers(con);
            fp.ShowDialog();
        }
  
        private void Form1_Load(object sender, EventArgs e)
        {
            MyLoad();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ivoices fp = new ivoices(con);
            fp.ShowDialog();
        }
    }
}
