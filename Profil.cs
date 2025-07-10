using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Project_Akhir_PBE
{
    public partial class Profil : Form
    {
        private string connString = @"Data Source=localhost;Initial Catalog=CampusSchedulerDB;Integrated Security=True";

        public Profil()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
            txtPasswordBaru.PasswordChar = '*';
        }

        private void Profil_Load(object sender, EventArgs e)
        {
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT Username, Password, NIM, Email FROM tb_user WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", Session.UserID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtUsername.Text = reader["Username"].ToString();
                    txtPassword.Text = reader["Password"].ToString();
                    txtNIM.Text = reader["NIM"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                }
                reader.Close();
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // Validasi input (opsional)
            if (txtUsername.Text == "" || txtNIM.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Data tidak boleh kosong!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE tb_user SET Username=@Username, NIM=@NIM, Email=@Email, Password=@Password WHERE UserID=@UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text); // gunakan password terbaru
                cmd.Parameters.AddWithValue("@UserID", Session.UserID);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Profil berhasil disimpan!");
            }
        }

        private void btnGantiPassword_Click(object sender, EventArgs e)
        {
            string newPass = Microsoft.VisualBasic.Interaction.InputBox("Masukkan password baru:", "Ganti Password", "");
            if (string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Password baru tidak boleh kosong.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE tb_user SET Password=@Password WHERE UserID=@UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Password", newPass);
                cmd.Parameters.AddWithValue("@UserID", Session.UserID);
                conn.Open();
                cmd.ExecuteNonQuery();
                txtPassword.Text = newPass;
                MessageBox.Show("Password berhasil diganti!");
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            LoadUserProfile();
        }

        // Navigasi
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dasboard dashboard = new Dasboard();
            this.Hide();
            dashboard.Show();
        }

        private void btnJadwalKuliah_Click(object sender, EventArgs e)
        {
            Jadwal_Kuliah jadwal = new Jadwal_Kuliah();
            this.Hide();
            jadwal.Show();
        }

        private void btnEventCampus_Click(object sender, EventArgs e)
        {
            Event_Campus eventCampus = new Event_Campus();
            this.Hide();
            eventCampus.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            // Validasi input (opsional)
            if (txtUsername.Text == "" || txtNIM.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Data tidak boleh kosong!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE tb_user SET Username=@Username, NIM=@NIM, Email=@Email, Password=@Password WHERE UserID=@UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text); // gunakan password terbaru
                cmd.Parameters.AddWithValue("@UserID", Session.UserID);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Profil berhasil disimpan!");
            }
        }

        private void btnGantiPassword_Click_1(object sender, EventArgs e)
        {
            string newPass = txtPasswordBaru.Text.Trim();

            if (string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Password baru tidak boleh kosong.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE tb_user SET Password=@Password WHERE UserID=@UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Password", newPass);
                cmd.Parameters.AddWithValue("@UserID", Session.UserID);
                conn.Open();
                cmd.ExecuteNonQuery();
                txtPassword.Text = newPass;      // update di form
                txtPasswordBaru.Text = "";       // kosongkan input baru
                MessageBox.Show("Password berhasil diganti!");
            }
        }

        private void btnBatal_Click_1(object sender, EventArgs e)
        {
            LoadUserProfile();
        }
    }
}
