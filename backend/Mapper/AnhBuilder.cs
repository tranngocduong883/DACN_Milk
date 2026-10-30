using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class AnhBuilder
    {
        public static AnhResponse ToResponse(AnhSp img)
        {
            return new AnhResponse
            {
                MaAnh = img.MaAnh,
                MaSp = img.MaSp,
                DuongDanAnhSP = img.DuongDan
            };
        }
    }
}
