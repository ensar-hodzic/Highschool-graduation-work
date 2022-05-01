using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Ensi
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Tabela1();
        }

        private void Tabela1()
        {
            String query = "SELECT narudzbenica_id,datum_narudzbe FROM narudzbenica WHERE kupac_id='" + Login.kupacid + "'";
            try
            {
                MySqlConnection konekcija = new MySqlConnection(Login.konek);
                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(query, konekcija);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, konekcija);
                DataTable tabela = new DataTable();
                dataAdapter.Fill(tabela);
                dataGridView1.DataSource = tabela;
                konekcija.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Prikaz()
        {
            String query = "SELECT artikal_id,kolicina FROM stavka_narudzbenice WHERE narudzbenica_id='" + textBox1.Text + "'";
            try
            {
                MySqlConnection konekcija = new MySqlConnection(Login.konek);
                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(query, konekcija);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, konekcija);
                DataTable tabela = new DataTable();
                dataAdapter.Fill(tabela);
                dataGridView2.DataSource = tabela;
                konekcija.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            Total();
        }
        private void Total()
        {
            int total = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (Convert.ToString(row.Cells[0].Value) != "")
                {
                    MySqlConnection konekcija = new MySqlConnection(Login.konek);
                    konekcija.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT cijena FROM artikal WHERE artikal_id=" + Convert.ToString(row.Cells[0].Value), konekcija);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    total += Convert.ToInt32(reader[0]) * Convert.ToInt32(row.Cells["kolicina"].Value);
                    reader.Close();
                }
            }
            textBox2.Text = total.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Prikaz();
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void kreiranjeNarudžbeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 fr = new Form4();
            this.Hide();
            fr.Show();
        }
    }
}
