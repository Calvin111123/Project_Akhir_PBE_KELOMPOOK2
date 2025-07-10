using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Project_Akhir_PBE
{
    public partial class Registrasi : Form
    {
        string connString = @"Data Source=localhost;Initial Catalog=CampusSchedulerDB;Integrated Security=True";

        public Registrasi()
        {
            InitializeComponent();
        }

        private void btnDaftar_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();
            string nim = txtNIM.Text.Trim();

            // Validasi input
            if (username == "" || password == "" || email == "" || nim == "")
            {
                MessageBox.Show("Semua field harus diisi!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Cek username sudah ada
                    string cek = "SELECT COUNT(*) FROM tb_user WHERE Username = @username";
                    SqlCommand cmdCek = new SqlCommand(cek, conn);
                    cmdCek.Parameters.AddWithValue("@username", username);
                    int ada = (int)cmdCek.ExecuteScalar();
                    if (ada > 0)
                    {
                        MessageBox.Show("Username sudah terdaftar, gunakan username lain.");
                        return;
                    }

                    // Insert user baru
                    string query = "INSERT INTO tb_user (Username, Password, NIM, Email) VALUES (@username, @password, @nim, @email)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@nim", nim);
                    cmd.Parameters.AddWithValue("@email", email);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registrasi berhasil! Silakan login.");
                    // Setelah berhasil, kembali ke halaman login
                    this.Hide();
                    Login login = new Login();
                    login.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal registrasi: " + ex.Message);
                }
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            // Kembali ke login tanpa daftar
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void btnDaftar_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string nim = txtNIM.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Validasi
            if (username == "" || password == "" || nim == "" || email == "")
            {
                MessageBox.Show("Semua field harus diisi!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Cek username sudah ada
                    string cek = "SELECT COUNT(*) FROM tb_user WHERE Username = @username";
                    SqlCommand cmdCek = new SqlCommand(cek, conn);
                    cmdCek.Parameters.AddWithValue("@username", username);
                    int sudahAda = (int)cmdCek.ExecuteScalar();

                    if (sudahAda > 0)
                    {
                        MessageBox.Show("Username sudah terdaftar, gunakan username lain!");
                        return;
                    }

                    // Insert user baru
                    string insert = "INSERT INTO tb_user (Username, Password, NIM, Email) VALUES (@username, @password, @nim, @email)";
                    SqlCommand cmd = new SqlCommand(insert, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@nim", nim);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registrasi berhasil! Silakan login.");

                    // Kembali ke Login
                    this.Hide();
                    Login login = new Login();
                    login.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal registrasi: " + ex.Message);
                }
            }
        }

        private void btnBatal_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }
    }
}
