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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public int total=0;

        private void Form4_Load(object sender, EventArgs e)
        {
            Tabela1();
            dataGridView2.Columns.Add("artikal_id", "artikal_id");
            dataGridView2.Columns.Add("naziv_artikla", "naziv_artikla");
            dataGridView2.Columns.Add("kolicina", "kolicina");
            dataGridView2.Columns.Add("cijena", "cijena");
        }

        private void Tabela1()
        {
            String query = "SELECT a.artikal_id, a.naziv_artikla, a.vrsta_artikla, a.cijena, s.kolicina_stanje FROM artikal a, skladiste s WHERE a.artikal_id = s.artikal_id";
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

        private void button2_Click(object sender, EventArgs e)
        {
            Dodavanje();
        }

        private void Dodavanje()
        {
                int indexC = 0;
                MySqlConnection konekcija = new MySqlConnection(Login.konek);
                konekcija.Open();
                MySqlCommand cmd2 = new MySqlCommand("SELECT naziv_artikla,cijena FROM artikal WHERE artikal_id=" + textBox1.Text, konekcija);
                MySqlDataReader reader1 = cmd2.ExecuteReader();
                reader1.Read();
                if (reader1.HasRows)
                {
                    String naziv = reader1[0].ToString();
                    String cijena = reader1[1].ToString();
                    reader1.Close();
                    String query1 = "SELECT kolicina_stanje FROM skladiste WHERE artikal_id='" + textBox1.Text + "' ";
                    MySqlCommand cmd1 = new MySqlCommand(query1, konekcija);
                    MySqlDataReader reader = cmd1.ExecuteReader();
                    reader.Read();
                    if (Convert.ToInt32(textBox2.Text) <= Convert.ToInt32(reader[0]))
                    {
                        int kolicina = Convert.ToInt32(reader[0]) - Convert.ToInt32(textBox2.Text);
                        reader.Close();
                        String query = "UPDATE skladiste SET kolicina_stanje='" + kolicina.ToString() + "' " +
                            " WHERE artikal_id='" + textBox1.Text + "' ";
                        MySqlCommand cmd = new MySqlCommand(query, konekcija);
                        cmd.ExecuteNonQuery();
                        konekcija.Close();
                        if (dataGridView2.Rows.Count > 1)
                        {
                            foreach (DataGridViewRow row in dataGridView2.Rows)
                            {
                                if (Convert.ToString(row.Cells["artikal_id"].Value) == textBox1.Text)
                                {
                                    indexC = row.Index + 1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        if (indexC == 0)
                        {
                            var index = dataGridView2.Rows.Add();
                            dataGridView2.Rows[index].Cells[0].Value = textBox1.Text;
                            dataGridView2.Rows[index].Cells[1].Value = naziv;
                            dataGridView2.Rows[index].Cells[2].Value = textBox2.Text;
                            dataGridView2.Rows[index].Cells[3].Value = cijena;
                        }
                        else
                        {
                            dataGridView2.Rows[indexC - 1].Cells["kolicina"].Value = (Convert.ToInt32(dataGridView2.Rows[indexC - 1].Cells["kolicina"].Value) + Convert.ToInt32(textBox2.Text)).ToString();
                        }
                        Tabela1();
                        Total();
                    }
                    else
                    {
                        MessageBox.Show("Ne možete naručiti više artikala nego što se nalazi u skladištu");
                    }
                }
                else
                {
                    reader1.Close();
                    MessageBox.Show("Nepostojeći ID artikla");
                }
        }

        private void Brisanje()
        {
            try
            {
                int tkol=0;
                int index=0;
                String query1 = "SELECT kolicina_stanje FROM skladiste WHERE artikal_id='" + textBox1.Text + "' ";
                MySqlConnection konekcija = new MySqlConnection(Login.konek);
                konekcija.Open();
                MySqlCommand cmd1 = new MySqlCommand(query1, konekcija);
                MySqlDataReader reader = cmd1.ExecuteReader();
                reader.Read();
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells["artikal_id"].Value.ToString() == textBox1.Text)
                    {
                        index = row.Index;
                        tkol = Convert.ToInt32(row.Cells["kolicina"].Value);
                        break;
                    }
                }
                int kolicina = Convert.ToInt32(reader[0]) + tkol;
                reader.Close();
                String query = "UPDATE skladiste SET kolicina_stanje='" + kolicina.ToString() + "' " +
                    " WHERE artikal_id='" + textBox1.Text + "' ";
                MySqlCommand cmd = new MySqlCommand(query, konekcija);
                cmd.ExecuteNonQuery();
                konekcija.Close();
                dataGridView2.Rows.RemoveAt(index);
                Tabela1();
                Total();
            }
            catch (Exception e)
            {  
                MessageBox.Show(e.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Brisanje();
        }

        private void Total()
        {
            total=0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                total += Convert.ToInt32(row.Cells["cijena"].Value) * Convert.ToInt32(row.Cells["kolicina"].Value);
            }
            textBox3.Text = total.ToString();
        }

        private void Kreiranje()
        {
            MySqlConnection konekcija = new MySqlConnection(Login.konek);
            konekcija.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT narudzbenica_id FROM narudzbenica ORDER BY narudzbenica_id DESC LIMIT 1", konekcija);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int id = Convert.ToInt32(reader[0].ToString()) + 1;
            reader.Close();
            String query = "INSERT INTO narudzbenica VALUES (" + id.ToString() + ", " + Login.kupacid + ", '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            MySqlCommand cmd1 = new MySqlCommand(query, konekcija);
            cmd1.ExecuteNonQuery();
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (Convert.ToString(row.Cells[0].Value) != "")
                {
                    String query1 = "INSERT INTO stavka_narudzbenice(narudzbenica_id, artikal_id, kolicina) VALUES (" + id.ToString() + ", " + Convert.ToString(row.Cells[0].Value) + ", " + Convert.ToString(row.Cells[2].Value) + ")";
                    MySqlCommand cmd2 = new MySqlCommand(query1, konekcija);
                    cmd2.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Narudžba ID=" + id.ToString() + " kreirana");
            dataGridView2.Rows.Clear();
            Tabela1();
            konekcija.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kreiranje();
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void prikazNarudžbiIStavkiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 fr = new Form5();
            this.Hide();
            fr.Show();
        }

    }
}
