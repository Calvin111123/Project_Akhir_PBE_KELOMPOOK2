using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Project_Akhir_PBE
{
    public partial class Event_Campus : Form
    {
        private string connString = @"Data Source=localhost;Initial Catalog=CampusSchedulerDB;Integrated Security=True";
        private int selectedEventID = -1;

        public Event_Campus()
        {
            InitializeComponent();
        }

        private void Event_Campus_Load(object sender, EventArgs e)
        {
            // Atur DateTimePicker waktu hanya jam (pastikan sudah di-designer)
            dtpWaktu.Format = DateTimePickerFormat.Time;
            dtpWaktu.ShowUpDown = true; // Tidak tampil kalender
            LoadEventUser();
        }

        private void LoadEventUser()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT EventID, NamaEvent, Tanggal, Waktu, Tempat, Status, Keterangan FROM tb_eventkampus";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewEvent.DataSource = dt;
            }
        }

        private void btnTambahEvent_Click(object sender, EventArgs e)
        {
            if (txtNamaEvent.Text == "" || txtTempat.Text == "")
            {
                MessageBox.Show("Isi minimal Nama Event dan Tempat!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO tb_eventkampus (NamaEvent, Tanggal, Waktu, Tempat, Status, Keterangan) VALUES (@NamaEvent, @Tanggal, @Waktu, @Tempat, @Status, @Keterangan)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NamaEvent", txtNamaEvent.Text);
                cmd.Parameters.AddWithValue("@Tanggal", dtpTanggal.Value.Date);
                cmd.Parameters.AddWithValue("@Waktu", dtpWaktu.Value.ToString("HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Tempat", txtTempat.Text);
                cmd.Parameters.AddWithValue("@Status", txtStatus.Text);
                cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Event berhasil ditambah!");
            }
            LoadEventUser();
            ClearInputEvent();
        }

        private void btnEditEvent_Click(object sender, EventArgs e)
        {
            if (selectedEventID == -1)
            {
                MessageBox.Show("Pilih event yang akan diedit!");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE tb_eventkampus SET NamaEvent=@NamaEvent, Tanggal=@Tanggal, Waktu=@Waktu, Tempat=@Tempat, Status=@Status, Keterangan=@Keterangan WHERE EventID=@EventID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NamaEvent", txtNamaEvent.Text);
                cmd.Parameters.AddWithValue("@Tanggal", dtpTanggal.Value.Date);
                cmd.Parameters.AddWithValue("@Waktu", dtpWaktu.Value.ToString("HH:mm:ss"));
                cmd.Parameters.AddWithValue("@Tempat", txtTempat.Text);
                cmd.Parameters.AddWithValue("@Status", txtStatus.Text);
                cmd.Parameters.AddWithValue("@Keterangan", txtKeterangan.Text);
                cmd.Parameters.AddWithValue("@EventID", selectedEventID);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Event berhasil diubah!");
            }
            LoadEventUser();
            ClearInputEvent();
            selectedEventID = -1;
        }

        private void btnHapusEvent_Click(object sender, EventArgs e)
        {
            if (selectedEventID == -1)
            {
                MessageBox.Show("Pilih event yang akan dihapus!");
                return;
            }
            var confirm = MessageBox.Show("Yakin hapus event?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "DELETE FROM tb_eventkampus WHERE EventID = @EventID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EventID", selectedEventID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event berhasil dihapus!");
                }
                LoadEventUser();
                ClearInputEvent();
                selectedEventID = -1;
            }
        }

        private void dataGridViewEvent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewEvent.Rows[e.RowIndex];
                selectedEventID = Convert.ToInt32(row.Cells["EventID"].Value);
                txtNamaEvent.Text = row.Cells["NamaEvent"].Value.ToString();
                dtpTanggal.Value = Convert.ToDateTime(row.Cells["Tanggal"].Value);

                // Pastikan format waktu di DB adalah "HH:mm:ss"
                DateTime waktu;
                if (DateTime.TryParseExact(row.Cells["Waktu"].Value.ToString(), "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out waktu))
                    dtpWaktu.Value = waktu;
                else
                    dtpWaktu.Value = DateTime.Now; // fallback

                txtTempat.Text = row.Cells["Tempat"].Value.ToString();
                txtStatus.Text = row.Cells["Status"].Value.ToString();
                txtKeterangan.Text = row.Cells["Keterangan"].Value.ToString();
            }
        }

        private void ClearInputEvent()
        {
            txtNamaEvent.Text = "";
            txtTempat.Text = "";
            txtStatus.Text = "";
            txtKeterangan.Text = "";
            dtpTanggal.Value = DateTime.Now;
            dtpWaktu.Value = DateTime.Now;
        }

        // Navigasi (jika ada)
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

        private void dataGridViewEvent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewEvent.Rows[e.RowIndex];

                // Ambil data dari baris yang diklik
                selectedEventID = Convert.ToInt32(row.Cells["EventID"].Value);
                txtNamaEvent.Text = row.Cells["NamaEvent"].Value.ToString();
                dtpTanggal.Value = Convert.ToDateTime(row.Cells["Tanggal"].Value);

                // Ambil waktu dan parsing ke dtpWaktu (pastikan dtpWaktu = DateTimePicker Format Time)
                string waktuString = row.Cells["Waktu"].Value.ToString();
                DateTime waktu;
                if (DateTime.TryParse(waktuString, out waktu))
                    dtpWaktu.Value = waktu;
                else
                    dtpWaktu.Value = DateTime.Now; // fallback jika gagal parse

                txtTempat.Text = row.Cells["Tempat"].Value.ToString();
                txtStatus.Text = row.Cells["Status"].Value.ToString();
                txtKeterangan.Text = row.Cells["Keterangan"].Value.ToString();
            }
        }
    }
}
