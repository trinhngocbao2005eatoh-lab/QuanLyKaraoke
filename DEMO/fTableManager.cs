using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEMO
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();
            LoadRoom(); // Gọi hàm load phòng ngay khi mở Form
        }

        // TÁCH CODE RA THÀNH HÀM RIÊNG (Không để trong sự kiện Paint)
        void LoadRoom()
        {
            // 1. Xóa hết các nút cũ
            flpRoom.Controls.Clear();

            // 2. Lấy danh sách phòng từ Database
            string query = "SELECT * FROM Room";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            // 3. Vòng lặp tạo nút
            foreach (DataRow item in data.Rows)
            {
                Button btn = new Button();

                // --- Cài đặt giao diện ---
                btn.Width = 90;
                btn.Height = 90;
                btn.Text = item["name"] + Environment.NewLine + item["status"];
                btn.Font = new Font("Arial", 10, FontStyle.Bold);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.ForeColor = Color.White;

                // --- Xử lý màu sắc ---
                string status = item["status"].ToString();
                switch (status)
                {
                    case "Trống":
                        btn.BackColor = Color.Teal;
                        break;
                    case "Có người":
                        btn.BackColor = Color.Crimson;
                        break;
                    default:
                        btn.BackColor = Color.Gray;
                        break;
                }

                // --- Lưu dữ liệu vào nút ---
                btn.Tag = item; // Lưu dòng dữ liệu vào Tag

                // --- Gắn sự kiện click ---
                btn.Click += btn_Click;

                // --- Thêm vào giao diện ---
                flpRoom.Controls.Add(btn);
            }
        }

        // Sự kiện khi bấm vào nút phòng
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            // Lấy dữ liệu từ Tag ra (Dùng ép kiểu DataRow cho an toàn)
            DataRow row = btn.Tag as DataRow;

            int idRoom = (int)row["id"];
            string tenPhong = row["name"].ToString();
            string trangThai = row["status"].ToString();

            // Hiện thông báo test thử
            MessageBox.Show("Bạn chọn: " + tenPhong + "\nTrạng thái: " + trangThai);
        }

        
    }
}