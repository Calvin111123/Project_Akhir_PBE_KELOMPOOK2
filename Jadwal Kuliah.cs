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
    public partial class Jadwal_Kuliah : Form
    {
        private string connString = @"Data Source=localhost;Initial Catalog=CampusSchedulerDB;Integrated Security=True";
        private int selectedJadwalID = -1;

        public Jadwal_Kuliah()
        {
            InitializeComponent();
        }

        private void Jadwal_Kuliah_Load(object sender, EventArgs e)
        {
            comboHari.Items.Clear();
            comboHari.Items.AddRange(new string[] {
                "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu", "Minggu"
            });
            LoadJadwalUser();
        }

        private void LoadJadwalUser()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT JadwalID, Hari, JamMulai, JamSelesai, MataKuliah, Dosen, Ruangan, Keterangan FROM tb_jadwalkuliah WHERE UserID = @userId";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@userId", Session.UserID);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewJadwal.DataSource = dt;
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // Validasi
            if (comboHari.SelectedIndex == -1 || txtJamMulai.Text == "" || txtMataKuliah.Text == "")
            {
                MessageBox.Show("Pilih Hari, isi Jam Mulai dan Mata Kuliah!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO tb_jadwalkuliah (Hari, JamMulai, JamSelesai, MataKuliah, Dosen, Ruangan, Keterangan, UserID) VALUES (@Hari, @JamMulai, @JamSelesai, @MataKuliah, @Dosen, @Ruangan, @Keterangan, @UserID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Hari", comboHari.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@JamMulai", txtJamMulai.Text);
                cmd.Parameters.AddWithValue("@JamSelesai", txtJamSelesai.Text);
                cmd.Parameters.AddWithValue("@MataKuliah", txtMataKuliah.Text);
                cmd.Parameters.AddWithValue("@Dosen", txtDosen.Text);
                cmd.Parameters.AddWithValue("@Ruangan", txtRuangan.Text);
                cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);
                cmd.Parameters.AddWithValue("@UserID", Session.UserID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Jadwal berhasil ditambah!");
            }
            LoadJadwalUser();
            ClearInput();
        }

        private void ClearInput()
        {
            comboHari.SelectedIndex = -1;
            txtJamMulai.Text = "";
            txtJamSelesai.Text = "";
            txtMataKuliah.Text = "";
            txtDosen.Text = "";
            txtRuangan.Text = "";
            txtKeterangan.Text = "";
            selectedJadwalID = -1;
        }

        private void dataGridViewJadwal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewJadwal.Rows[e.RowIndex];
                selectedJadwalID = Convert.ToInt32(row.Cells["JadwalID"].Value);
                comboHari.SelectedItem = row.Cells["Hari"].Value.ToString();
                txtJamMulai.Text = row.Cells["JamMulai"].Value.ToString();
                txtJamSelesai.Text = row.Cells["JamSelesai"].Value.ToString();
                txtMataKuliah.Text = row.Cells["MataKuliah"].Value.ToString();
                txtDosen.Text = row.Cells["Dosen"].Value.ToString();
                txtRuangan.Text = row.Cells["Ruangan"].Value.ToString();
                txtKeterangan.Text = row.Cells["Keterangan"].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedJadwalID == -1)
            {
                MessageBox.Show("Pilih data yang akan diedit!");
                return;
            }
            if (comboHari.SelectedIndex == -1 || txtJamMulai.Text == "" || txtMataKuliah.Text == "")
            {
                MessageBox.Show("Pilih Hari, isi Jam Mulai dan Mata Kuliah!");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE tb_jadwalkuliah SET Hari=@Hari, JamMulai=@JamMulai, JamSelesai=@JamSelesai, MataKuliah=@MataKuliah, Dosen=@Dosen, Ruangan=@Ruangan, Keterangan=@Keterangan WHERE JadwalID=@JadwalID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Hari", comboHari.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@JamMulai", txtJamMulai.Text);
                cmd.Parameters.AddWithValue("@JamSelesai", txtJamSelesai.Text);
                cmd.Parameters.AddWithValue("@MataKuliah", txtMataKuliah.Text);
                cmd.Parameters.AddWithValue("@Dosen", txtDosen.Text);
                cmd.Parameters.AddWithValue("@Ruangan", txtRuangan.Text);
                cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);
                cmd.Parameters.AddWithValue("@JadwalID", selectedJadwalID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Jadwal berhasil diubah!");
            }
            LoadJadwalUser();
            ClearInput();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (selectedJadwalID == -1)
            {
                MessageBox.Show("Pilih data yang akan dihapus!");
                return;
            }
            var confirm = MessageBox.Show("Yakin hapus data?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "DELETE FROM tb_jadwalkuliah WHERE JadwalID = @JadwalID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@JadwalID", selectedJadwalID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Jadwal berhasil dihapus!");
                }
                LoadJadwalUser();
                ClearInput();
            }
        }

        // Navigasi tombol dan fungsi lain sesuai aplikasi kamu...
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dasboard dashboard = new Dasboard();
            this.Hide();
            dashboard.Show();
        }
        private void btnEventCampus_Click(object sender, EventArgs e)
        {
            Event_Campus eventCampus = new Event_Campus();
            this.Hide();
            eventCampus.Show();
        }
        private void btnProfil_Click(object sender, EventArgs e)
        {
            Profil profil = new Profil();
            this.Hide();
            profil.Show();
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            Dasboard dashboard = new Dasboard();
            this.Hide();
            dashboard.Show();
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (selectedJadwalID == -1)
            {
                MessageBox.Show("Pilih data yang akan diedit!");
                return;
            }
            if (comboHari.SelectedIndex == -1 || txtJamMulai.Text == "" || txtMataKuliah.Text == "")
            {
                MessageBox.Show("Pilih Hari, isi Jam Mulai dan Mata Kuliah!");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE tb_jadwalkuliah SET Hari=@Hari, JamMulai=@JamMulai, JamSelesai=@JamSelesai, MataKuliah=@MataKuliah, Dosen=@Dosen, Ruangan=@Ruangan, Keterangan=@Keterangan WHERE JadwalID=@JadwalID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Hari", comboHari.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@JamMulai", txtJamMulai.Text);
                cmd.Parameters.AddWithValue("@JamSelesai", txtJamSelesai.Text);
                cmd.Parameters.AddWithValue("@MataKuliah", txtMataKuliah.Text);
                cmd.Parameters.AddWithValue("@Dosen", txtDosen.Text);
                cmd.Parameters.AddWithValue("@Ruangan", txtRuangan.Text);
                cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);
                cmd.Parameters.AddWithValue("@JadwalID", selectedJadwalID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Jadwal berhasil diubah!");
            }
            LoadJadwalUser();
            ClearInput();
        }

        private void btnHapus_Click_1(object sender, EventArgs e)
        {
            if (selectedJadwalID == -1)
            {
                MessageBox.Show("Pilih data yang akan dihapus!");
                return;
            }
            var confirm = MessageBox.Show("Yakin hapus data?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "DELETE FROM tb_jadwalkuliah WHERE JadwalID = @JadwalID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@JadwalID", selectedJadwalID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Jadwal berhasil dihapus!");
                }
                LoadJadwalUser();
                ClearInput();
            }
        }
    }
}
