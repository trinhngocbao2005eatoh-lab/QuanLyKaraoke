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

namespace DEMO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string passWord = txtPassWord.Text;

            if (Login(userName, passWord)) // Gọi hàm kiểm tra Login ở dưới
            {
                // Nếu đúng: Mở form quản lý chính (fTableManager)
                fTableManager f = new fTableManager();
                this.Hide(); // Ẩn form đăng nhập đi
                f.ShowDialog(); // Hiện form chính
                this.Show(); // Khi tắt form chính thì hiện lại form đăng nhập
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!", "Thông báo");
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }

        }
        // Xử lý sự kiện khi người dùng bấm nút X đỏ trên góc phải form
        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true; // Hủy lệnh thoát nếu người dùng bấm Cancel
            }
        }
        bool Login(string userName, string passWord)
        {
            // Thay đoạn text này bằng Connection String bạn copy ở Bước 2
            // Lưu ý: Thêm ký tự @ đằng trước để chuỗi không bị lỗi ký tự đặc biệt
            string connectionSTR = @"Data Source=NgocBao\SQLEXPRESS02;Initial Catalog=QuanLyKaraoke;Integrated Security=True";

            // Dùng câu lệnh SQL có tham số (@u, @p) để tránh bị hack (SQL Injection)
            string query = "SELECT * FROM Account WHERE UserName = @u AND PassWord = @p";

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                try
                {
                    connection.Open(); // Mở cổng kết nối

                    SqlCommand command = new SqlCommand(query, connection);

                    // Truyền dữ liệu vào tham số
                    command.Parameters.AddWithValue("@u", userName);
                    command.Parameters.AddWithValue("@p", passWord);

                    // ExecuteScalar: Thường dùng để lấy ô dữ liệu đầu tiên (kiểm tra có dòng nào không)
                    // Hoặc dùng SqlDataAdapter để lấy cả bảng dữ liệu
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    // Nếu bảng data có dữ liệu (> 0 dòng) nghĩa là tài khoản đúng
                    return data.Rows.Count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
                    return false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        void ClearLogin()
        {
            txtUserName.Text = "";
            txtPassWord.Text = "";
        }
    }
}
