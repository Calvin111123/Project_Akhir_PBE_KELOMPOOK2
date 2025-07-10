using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Akhir_PBE
{
    public partial class Login : Form
    {
        string connString = @"Data Source=localhost;Initial Catalog=CampusSchedulerDB;Integrated Security=True";

        public Login()
        {
            InitializeComponent();

            
            txtUsername.Text = "Username";
            txtUsername.ForeColor = Color.Gray;
            txtUsername.Enter += txtUsername_Enter;
            txtUsername.Leave += txtUsername_Leave;

            
            txtPassword.Text = "Password";
            txtPassword.ForeColor = Color.Gray;
            txtPassword.UseSystemPasswordChar = false;
            txtPassword.Enter += txtPassword_Enter;
            txtPassword.Leave += txtPassword_Leave;
        }

        // ---- Placeholder Username
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Username")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }
        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Username";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        // ---- Placeholder Password
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Password";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Tidak boleh pakai placeholder
            if (username == "" || password == "" || username == "Username" || password == "Password")
            {
                MessageBox.Show("Username dan Password harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM tb_user WHERE Username = @username AND Password = @password";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        MessageBox.Show("Login berhasil! Selamat datang, " + reader["Username"].ToString());

                        // Simpan data user ke Session
                        Session.UserID = Convert.ToInt32(reader["UserID"]);
                        Session.Username = reader["Username"].ToString();
                        Session.NIM = reader["NIM"].ToString();
                        Session.Email = reader["Email"].ToString();

                        reader.Close();

                        Dasboard dashboard = new Dasboard();
                        this.Hide();
                        dashboard.Show();
                    }
                    else
                    {
                        reader.Close();
                        MessageBox.Show("Username atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Koneksi database gagal: " + ex.Message);
                }
            }
        }

        private void btnRegistrasi_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registrasi reg = new Registrasi();
            reg.Show();
        }

        // Contoh Event jika ada panel
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void Login_Load(object sender, EventArgs e) { }

        private void btnRegristasi_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registrasi reg = new Registrasi();
            reg.Show();
        }
    }

    public static class Session
    {
        public static int UserID;
        public static string Username;
        public static string NIM;
        public static string Email;
    }
}