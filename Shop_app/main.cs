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
    public partial class main : Form
    {
        public NpgsqlConnection con;
        public void MyLoad()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            con = new NpgsqlConnection("Server=localhost; Port = 5432; UserID=postgres; Password = postpass; Database = sklad_db1");
            con.Open();
        }
        public main()
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
            clients fp = new clients(con);
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


        private void button3_Click_1(object sender, EventArgs e)
        {
            Price_list fp = new Price_list(con);
            fp.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sklad fp = new sklad(con);
            fp.ShowDialog();
        }
    }
}
