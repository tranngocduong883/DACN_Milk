namespace DACN_H_P.Dtos.Response
{
    public class DanhGiaResponse
    {
        public string MaDanhGia { get; set; } = null!;
        public string MaSp { get; set; } = null!;
        public string TenSp { get; set; } = null!;
        public string MaTaiKhoan { get; set; } = null!;
        public string TenNguoiDung { get; set; } = null!;
        public int SoSao { get; set; }
        public string BinhLuan { get; set; } = null!;
        public DateTime NgayCapNhat { get; set; }
        public string TrangThai { get; set; } = null!;
    }
}
