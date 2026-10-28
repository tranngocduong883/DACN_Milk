using DACN_H_P.Model;

namespace DACN_H_P.Repository
{
    public interface IKhuyenMaiRepository
    {
        Task<IEnumerable<KhuyenMai>> GetAllKM();
        Task<KhuyenMai?> GetKMByMaKM(string makm);
        Task<bool> PostKM(KhuyenMai km);
        Task<bool> UpdateKM(KhuyenMai km);
        Task<bool> DeleteKM(string makm);
        Task<IEnumerable<ChiTietKm>> GetChiTietKmByMaKM(string maKM);
        Task<IEnumerable<ChiTietKm>> GetChiTietKmByMaSP(string maSP);
        Task<bool> PostChiTietKm(ChiTietKm ctkm);
        Task<bool> UpdateChiTietKm(ChiTietKm ctkm);
        Task<bool> DeleteChiTietKm(string maKM, string maSP);
        Task<ChiTietKm?> GetChiTietKm(string maKM, string maSP);
        Task<bool> CheckKmExist(string maKM);
        Task<bool> CheckSpExist(string maSp);
        Task<bool> CheckChiTietExist(string maKM, string maSp);
    }
}
