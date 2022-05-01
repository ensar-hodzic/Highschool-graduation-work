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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }


        private void Tabela()
        {
            String query = "SELECT n.narudzbenica_id, n.kupac_id, k.ime, k.prezime, n.datum_narudzbe FROM narudzbenica n, kupac k WHERE n.kupac_id=k.kupac_id";
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

        private void Form3_Load(object sender, EventArgs e)
        {
            Tabela();
        }

        private void Brisanje()
        {
            String query = "DELETE FROM narudzbenica WHERE narudzbenica_id="+textBox1.Text;
            String query1 = "SELECT COUNT(*) FROM stavka_narudzbenice WHERE narudzbenica_id=" + textBox1.Text;
            MySqlConnection konekcija = new MySqlConnection(Login.konek);
            konekcija.Open();
            MySqlCommand cmd1 = new MySqlCommand(query1, konekcija);
            MySqlDataReader reader = cmd1.ExecuteReader();
            reader.Read();
            int red = Convert.ToInt32(reader[0]);
            reader.Close();
            for(int i=red ;i>0;i--)
            {
                // SELECT stavka_id, artikal_id, kolicina
                String query2 = "SELECT stavka_id, artikal_id, kolicina FROM stavka_narudzbenice WHERE narudzbenica_id=" + textBox1.Text+" LIMIT 1";
                MySqlCommand cmd2 = new MySqlCommand(query2, konekcija);
                MySqlDataReader reader1 = cmd2.ExecuteReader();
                reader1.Read();
                String stavka_id = reader1[0].ToString();
                String artikal_id = reader1[1].ToString();
                String kolicina1 = reader1[2].ToString();
                reader1.Close();
                // SELECT kolicina_stanje FROM skladiste
                String query3 = "SELECT kolicina_stanje FROM skladiste WHERE artikal_id=" + artikal_id.ToString();
                MySqlCommand cmd3 = new MySqlCommand(query3, konekcija);
                MySqlDataReader reader2 = cmd3.ExecuteReader();
                reader2.Read();
                String kolicina2 = reader2[0].ToString();
                reader2.Close();
                //UPDATE
                int kolicina = Convert.ToInt32(kolicina1)+Convert.ToInt32(kolicina2);
                String query4 = "UPDATE skladiste SET kolicina_stanje='" + kolicina.ToString() + "' " +
                    " WHERE artikal_id='" + artikal_id.ToString() + "' ";
                MySqlCommand cmd4 = new MySqlCommand(query4, konekcija);
                cmd4.ExecuteNonQuery();
                // DELETE FROM stavka_narudzbenice
                String query5 = "DELETE FROM stavka_narudzbenice WHERE stavka_id=" + stavka_id.ToString();
                MySqlCommand cmd5 = new MySqlCommand(query5, konekcija);
                cmd5.ExecuteNonQuery();
            }
            MySqlCommand cmd = new MySqlCommand(query, konekcija);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Narudzba ID " + textBox1.Text + " izbrisana");
            konekcija.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Brisanje();
            Tabela();
        }

        private void kreiranjeAžuriranjeNovogKupcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            this.Hide();
            fr.Show();
        }

        private void dodavanjeAžuriranjeArtikalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fr = new Form2();
            this.Hide();
            fr.Show();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
