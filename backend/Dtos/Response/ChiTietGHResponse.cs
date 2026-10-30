namespace DACN_H_P.Dtos.Response
{
    public class ChiTietGHResponse
    {
        public string MaSp { get; set; } = null!;
        public string TenSp { get; set; } = null!;
        public string? Anh { get; set; }
        public int SoLuong { get; set; }

        public decimal GiaBanDau { get; set; }
        public decimal GiaDatHang{ get; set; }
        public decimal ThanhTien => SoLuong * GiaDatHang;    
    }
}
