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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Tabela();
        }

        private void Tabela()
        {
            String query = "SELECT * FROM kupac";
            if (textBox1.Text != "" && textBox2.Text=="")
            {
                query += " WHERE ime LIKE '" + textBox1.Text + "%' ";
            }
            if (textBox2.Text != "" && textBox1.Text == "")
            {
                query += " WHERE prezime LIKE '" + textBox2.Text + "%' ";
            }
            if (textBox2.Text != "" && textBox1.Text != "")
            {
                query += " WHERE ime LIKE '" + textBox2.Text + "%' AND prezime LIKE '" + textBox1.Text + "%' ";
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tabela();
        }

        private void Azuriranje()
        {
            try
            {
                String query = "UPDATE kupac SET ime='" + textBox3.Text + "', " +
                    " prezime='" + textBox4.Text + "', " +
                    " grad='" + textBox5.Text + "', " +
                    " adresa='" + textBox6.Text + "', " +
                    " telefon='" + textBox7.Text + "', " +
                    " username='" + textBox8.Text + "', " +
                    " password='" + textBox9.Text + "' " +
                    " WHERE kupac_id='" + textBox10.Text + "' ";
                MySqlConnection konekcija = new MySqlConnection(Login.konek);
                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(query, konekcija);
                cmd.ExecuteNonQuery();
                MessageBox.Show("ID "+textBox10.Text+" podaci ažurirani");
                konekcija.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Kreiranje()
        {
            try
            {
                String query = "INSERT INTO kupac(kupac_id, ime, prezime, grad, adresa, telefon, username, password)  VALUES " +
                    " ('"+textBox10.Text+"', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', " +
                    " '" + textBox6.Text + "', '" + textBox7.Text + "', " +
                    " '" + textBox8.Text + "', '" + textBox9.Text + "') ";
                MySqlConnection konekcija = new MySqlConnection(Login.konek);
                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(query, konekcija);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Kupac ID "+textBox10.Text+" dodan");
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

        private void button2_Click(object sender, EventArgs e)
        {
            Kreiranje();
            Tabela();
        }

        private void dodavanjeAžuriranjeArtikalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fr = new Form2();
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
