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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            Tabela();
        }

        private void Tabela()
        {
            String query = "SELECT a.artikal_id, a.naziv_artikla, a.vrsta_artikla, a.cijena, s.kolicina_stanje FROM artikal a, skladiste s WHERE a.artikal_id = s.artikal_id";
            if (textBox1.Text != "" && textBox2.Text == "")
            {
                query += " AND a.artikal_id LIKE '" + textBox1.Text + "%' ";
            }
            if (textBox2.Text != "" && textBox1.Text == "")
            {
                query += " AND a.naziv_artikla LIKE '" + textBox2.Text + "%' ";
            }
            if (textBox2.Text != "" && textBox1.Text != "")
            {
                query += " AND a.artikal_id LIKE '" + textBox2.Text + "%' AND a.naziv_artikla LIKE '" + textBox1.Text + "%' ";
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            Azuriranje();
            Tabela();
        }

        private void Azuriranje()
        {
            try
            {
                String query1 = "SELECT kolicina_stanje FROM skladiste WHERE artikal_id='" + textBox7.Text + "' ";
                MySqlConnection konekcija = new MySqlConnection(Login.konek);
                konekcija.Open();
                MySqlCommand cmd1 = new MySqlCommand(query1, konekcija);
                MySqlDataReader reader = cmd1.ExecuteReader();
                reader.Read();
                int kolicina = Convert.ToInt32(reader[0])+Convert.ToInt32(numericUpDown1.Value);
                reader.Close();
                String query = "UPDATE skladiste SET kolicina_stanje='" + kolicina.ToString() + "' " +
                    " WHERE artikal_id='" + textBox7.Text + "' ";
                MySqlCommand cmd = new MySqlCommand(query, konekcija);
                cmd.ExecuteNonQuery();
                MessageBox.Show("ID " + textBox7.Text + " podaci ažurirani");
                konekcija.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kreiranje();
            Tabela();
        }
        private void Kreiranje()
        {
            try
            {
                String query = "INSERT INTO artikal(artikal_id, naziv_artikla, vrsta_artikla, cijena)  VALUES " +
                    " ('" + textBox7.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "')";
                String query1 = "INSERT INTO skladiste(id, artikal_id, kolicina_stanje) VALUES ('" + textBox7.Text + "', '" + textBox7.Text + "', '" + textBox6.Text + "') ";
                MySqlConnection konekcija = new MySqlConnection(Login.konek);
                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(query, konekcija);
                cmd.ExecuteNonQuery();
                MySqlCommand cmd1 = new MySqlCommand(query1, konekcija);
                cmd1.ExecuteNonQuery();
                MessageBox.Show("Artikal ID " + textBox7.Text + " dodan");
                konekcija.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tabela();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void kreiranjeAžuriranjeNovogKupcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            this.Hide();
            fr.Show();
        }

        private void prikazBrisanjeNarudžbeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 fr = new Form3();
            this.Hide();
            fr.Show();
        }
    }
}
