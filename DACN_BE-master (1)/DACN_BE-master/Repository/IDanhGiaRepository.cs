using DACN_H_P.Model;

namespace DACN_H_P.Repository
{
    public interface IDanhGiaRepository
    {
        // CRUD cơ bản
        Task CreateAsync(DanhGiaSp dg);
        Task UpdateAsync(DanhGiaSp dg);
        Task DeleteAsync(DanhGiaSp dg);

        // Get Data
        Task<DanhGiaSp?> GetByMaDanhGia(string maDanhGia);
        Task<List<DanhGiaSp>> GetAllForAdmin(); // Lấy tất cả
        Task<List<DanhGiaSp>> GetByMaSp(string maSp); // Chỉ lấy cái Đã Duyệt để hiện lên Web

        // Logic nghiệp vụ
        Task<bool> CheckUserBoughtProduct(string maTk, string maSp); // Check đã mua chưa
        Task<bool> CheckUserReviewed(string maTk, string maSp); // Check đã đánh giá chưa (nếu muốn chặn spam 1 người đánh giá 10 lần)
    }
}
