using DACN_H_P.Model;

namespace DACN_H_P.Repository
{
    public interface IDanhMucRepository
    {
        Task<IEnumerable<DanhMuc>> GetAllAsync();

        Task<DanhMuc?> GetByMaDMAsync(string maDM);
        Task<IEnumerable<DanhMuc>> GetByTenDMAsync(string tenDM);
        Task<DanhMuc?> PostAsync(DanhMuc dm);
        Task<DanhMuc?> EditAsync(DanhMuc dm);
        Task<bool> DeleteAsync(string madm);
    }
}
