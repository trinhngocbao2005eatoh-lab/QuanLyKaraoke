using System;
using System.Data;
using System.Data.SqlClient; // Thư viện bắt buộc để dùng SQL
using System.Linq;

namespace DEMO
{
    public class DataProvider
    {
        // 1. Tạo Singleton (Để đảm bảo chỉ có duy nhất 1 kết nối chạy trong chương trình)
        private static DataProvider instance;

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private DataProvider() { }

        // 2. Chuỗi kết nối (QUAN TRỌNG NHẤT)
        // Bạn nhớ sửa lại tên Server (Data Source) cho đúng máy bạn nhé!
        private string connectionSTR = @"Data Source=NgocBao\SQLEXPRESS02;Initial Catalog=QuanLyKaraoke;Integrated Security=True";

        // 3. Hàm chạy câu lệnh SELECT (Lấy dữ liệu dạng bảng)
        // Ví dụ: Lấy danh sách phòng, danh sách món ăn
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open(); // Mở cổng
                SqlCommand command = new SqlCommand(query, connection);
                AddParameter(command, query, parameter); // Xử lý tham số an toàn
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data); // Đổ dữ liệu vào bảng
                connection.Close(); // Đóng cổng
            }
            return data;
        }

        // 4. Hàm chạy câu lệnh INSERT, UPDATE, DELETE (Thay đổi dữ liệu)
        // Ví dụ: Thêm phòng mới, Xóa món ăn, Sửa giá tiền
        // Trả về: Số dòng thành công (lớn hơn 0 là thành công)
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                AddParameter(command, query, parameter);
                data = command.ExecuteNonQuery();
                connection.Close();
            }
            return data;
        }

        // 5. Hàm chạy câu lệnh lấy 1 ô dữ liệu duy nhất
        // Ví dụ: Đếm số lượng, Tính tổng tiền
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                AddParameter(command, query, parameter);
                data = command.ExecuteScalar();
                connection.Close();
            }
            return data;
        }

        // Hàm phụ: Tách các tham số để truyền vào SQL (Chống hack SQL Injection)
        // Giúp bạn viết query kiểu: "SELECT * FROM Account WHERE User = @u"
        private void AddParameter(SqlCommand command, string query, object[] parameter)
        {
            if (parameter != null)
            {
                string[] listPara = query.Split(' ');
                int i = 0;
                foreach (string item in listPara)
                {
                    if (item.Contains('@'))
                    {
                        command.Parameters.AddWithValue(item, parameter[i]);
                        i++;
                    }
                }
            }
        }
    }
}