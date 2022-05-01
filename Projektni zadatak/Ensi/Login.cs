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
    public partial class Login : Form
    {
        public static String konek = "Server=localhost; Port=3306; " +
            "Database=projektni; Uid=root; Pwd=";
        public static String kupacid;

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            String username = textBox1.Text;
            String password = textBox2.Text;
            String query = "SELECT password, CONCAT(ime,' ',prezime), kupac_id FROM kupac WHERE username='" + username + "' ";
            try
            {
                MySqlConnection konekcija = new MySqlConnection(konek);
                konekcija.Open();
                MySqlCommand cmd = new MySqlCommand(query, konekcija);
                cmd.ExecuteNonQuery();
                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                reader.Read();
                if (!reader.HasRows)
                {
                    errorProvider1.SetError(textBox1, "Unijeli ste nepostojeći username");
                }
                else
                {
                    String passwordB = reader[0].ToString();
                    String ime = reader[1].ToString();
                    kupacid = reader[2].ToString();
                    if (password == passwordB)
                    {
                        if (kupacid == "1")
                        {
                            MessageBox.Show("-ADMIN PRIJAVA- Dobrodošli: " + ime);
                            Form1 fr = new Form1();
                            this.Hide();
                            fr.Show();
                        }
                        else
                        {
                            MessageBox.Show("Dobrodošli: " + ime);
                            Form4 fr = new Form4();
                            this.Hide();
                            fr.Show();
                        }
                    }
                    else
                    {
                        errorProvider1.SetError(textBox2, "Unijeli ste pogrešnu šifru");
                    }
                }
                reader.Close();
                konekcija.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}