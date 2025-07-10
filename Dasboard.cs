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
    public partial class Dasboard : Form
    {
        private string connString = @"Data Source=localhost;Initial Catalog=CampusSchedulerDB;Integrated Security=True";

        public Dasboard()
        {
            InitializeComponent();
        }

        private void Dasboard_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Load Dashboard Dipanggil!");
            LoadJadwalHariIni();
            LoadEventMendatang();
        }

        // 1. Tampilkan Jadwal Hari Ini (khusus user login)
        private void LoadJadwalHariIni()
        {
            int userId = Session.UserID;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT Hari, MataKuliah, JamMulai, JamSelesai, Ruangan
                         FROM tb_jadwalkuliah
                         WHERE UserID = @userId
                         ORDER BY 
                            CASE Hari
                                WHEN 'Senin' THEN 1
                                WHEN 'Selasa' THEN 2
                                WHEN 'Rabu' THEN 3
                                WHEN 'Kamis' THEN 4
                                WHEN 'Jumat' THEN 5
                                WHEN 'Sabtu' THEN 6
                                WHEN 'Minggu' THEN 7
                                ELSE 8 END, JamMulai";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@userId", userId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewJadwalDashboard.DataSource = dt;

                // == Notifikasi Jadwal Terdekat (versi satu label, tidak hanya hari ini, urutkan yang terdekat di seluruh minggu) ==
                string notifJadwal = "Tidak ada jadwal kuliah!";
                if (dt.Rows.Count > 0)
                {
                    // Ambil jadwal terdekat (pertama diurutkan)
                    DataRow next = dt.Rows[0];
                    notifJadwal = $"Jadwal terdekat: {next["Hari"]} - {next["MataKuliah"]} jam {next["JamMulai"]} di {next["Ruangan"]}";
                }
                lblNotifikasi.Text = notifJadwal;
            }
        }
        // 2. Tampilkan Event Mendatang (tanggal >= hari ini)
        private void LoadEventMendatang()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT NamaEvent, Tanggal, Waktu, Tempat, Status FROM tb_eventkampus WHERE Tanggal >= CAST(GETDATE() AS DATE) ORDER BY Tanggal, Waktu";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewEventDashboard.DataSource = dt;

                // Notifikasi Event Terdekat (versi satu label, selalu ada jika ada event)
                string notifEvent = "Tidak ada event kampus!";
                if (dt.Rows.Count > 0)
                {
                    // Cari event terdekat dari sekarang (gabungkan tanggal dan waktu)
                    DateTime now = DateTime.Now;
                    var nextEvent = dt.AsEnumerable()
                        .Select(r => {
                            string tgl = r["Tanggal"].ToString();
                            string waktu = r["Waktu"].ToString();
                            if (waktu.Length == 5) waktu += ":00"; // Format ke HH:mm:ss jika perlu
                            DateTime dtEvent;
                            bool ok = DateTime.TryParse($"{tgl} {waktu}", out dtEvent);
                            return new { Row = r, EventDT = dtEvent, Ok = ok };
                        })
                        .Where(x => x.Ok && x.EventDT >= now)
                        .OrderBy(x => x.EventDT)
                        .Select(x => x.Row)
                        .FirstOrDefault();

                    // Jika tidak ada yang >= sekarang, ambil paling awal (biar tetap tampil)
                    if (nextEvent == null)
                        nextEvent = dt.Rows[0];

                    notifEvent = $"Event terdekat: {nextEvent["NamaEvent"]} pada {Convert.ToDateTime(nextEvent["Tanggal"]).ToString("dd/MM/yyyy")} jam {nextEvent["Waktu"].ToString().Substring(0, 5)} di {nextEvent["Tempat"]}";
                }
                lblNotifikasiEvent.Text = notifEvent;
            }
        }

        // 3. Tampilkan Notifikasi (jadwal terdekat & event hari ini)
       
        // Navigasi ke halaman lain
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

        private void btnProfil_Click(object sender, EventArgs e)
        {
            Profil profil = new Profil();
            this.Hide();
            profil.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dasboard dashboard = new Dasboard();
            this.Hide();
            dashboard.Show();
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

        private void btnJadwalKuliah_Click_1(object sender, EventArgs e)
        {
            Jadwal_Kuliah jadwal = new Jadwal_Kuliah();
            this.Hide();
            jadwal.Show();
        }

        private void btnEventCampus_Click_1(object sender, EventArgs e)
        {
            Event_Campus eventCampus = new Event_Campus();
            this.Hide();
            eventCampus.Show();
        }

        private void Dasboard_Load_1(object sender, EventArgs e)
        {
            MessageBox.Show("Load Dashboard Dipanggil!");
            LoadJadwalHariIni();
            LoadEventMendatang();
        }

        private void groupBoxNotifikasi_Enter(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        // ... tombol navigasi lain bisa ditambah di sini ...
    }
}
