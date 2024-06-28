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
using static Shop_app.ivoices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Excel = Microsoft.Office.Interop.Excel;


namespace Shop_app
{
    public partial class Price_list : Form
    {
        Microsoft.Office.Interop.Excel.Application xlApp;
        Microsoft.Office.Interop.Excel.Worksheet xlSheet;
        Microsoft.Office.Interop.Excel.Range xlSheetRange;
        public NpgsqlConnection con;
        int id;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public void Update()
        {
            String sql = "Select * from price_list";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dg.DataSource = dt;
            dg.Columns[0].HeaderText = "id";
            dg.Columns[1].HeaderText = "Стоимость товара";
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
        public Price_list(NpgsqlConnection con)
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

        private void Price_list_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void обновитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string sql = string.Format("UPDATE price_list set cost = '{0}'where id_t = '{1}'", Convert.ToInt32(textBox2.Text), id);
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
                int id = (int)dg.CurrentRow.Cells["id_t"].Value;
                NpgsqlCommand command = new NpgsqlCommand("Delete from price_list where id_t = :id", con);
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                Update();
            }
        }

        private void dg_SelectionChanged_1(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            xlApp = new Excel.Application();
            String sql = "Select id_t, tovar.name, cost from price_list JOIN tovar on price_list.id_t=tovar.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            xlApp.Workbooks.Add(Type.Missing);

            //делаем временно неактивным документ
            xlApp.Interactive = false;
            xlApp.EnableEvents = false;

            //выбираем лист на котором будем работать (Лист 1)
            xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
            //Название листа
            xlSheet.Name = "Данные";

            //Выгрузка данных

            int collInd = 0;
            int rowInd = 0;
            string data = "";

            //называем колонки
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data = dt.Columns[i].ColumnName.ToString();
                xlSheet.Cells[1, i + 1] = data;

                //выделяем первую строку
                xlSheetRange = xlSheet.get_Range("A1:Z1", Type.Missing);

                //делаем полужирный текст и перенос слов
                xlSheetRange.WrapText = true;
                xlSheetRange.Font.Bold = true;
            }

            //заполняем строки
            for (rowInd = 0; rowInd < dt.Rows.Count; rowInd++)
            {
                for (collInd = 0; collInd < dt.Columns.Count; collInd++)
                {
                    data = dt.Rows[rowInd].ItemArray[collInd].ToString();
                    xlSheet.Cells[rowInd + 2, collInd + 1] = data;
                }
            }

            //выбираем всю область данных
            xlSheetRange = xlSheet.UsedRange;

            //выравниваем строки и колонки по их содержимому
            xlSheetRange.Columns.AutoFit();
            xlSheetRange.Rows.AutoFit();
            xlApp.Visible = true;

            xlApp.Interactive = true;
            xlApp.ScreenUpdating = true;
            xlApp.UserControl = true;
        }
    }
}
