using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Mapper;
using DACN_H_P.Model;
using DACN_H_P.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration; // Dùng để đọc appsettings
using Microsoft.IdentityModel.Tokens;     // Dùng cho JWT
using System.IdentityModel.Tokens.Jwt;    // Dùng cho JWT
using System.Security.Claims;             // Dùng cho JWT
using System.Text;                        // Dùng cho JWT

namespace DACN_H_P.Service.Impl
{
    public class TaiKhoanService:ITaiKhoanService
    {
        private readonly ITaiKhoanRepository _repo;

        private readonly PasswordHasher<TaiKhoan> _hasher = new();

        private readonly IConfiguration _config;

        public TaiKhoanService(ITaiKhoanRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<(bool, string)> DangKyAsync(DangKyRequest request)
        {
            var exist = await _repo.GetByTenDangNhapAsync(request.TenDangNhap);
            if (exist != null)
            {
                return (false, "Tên đăng nhập đã tồn tại");
            }
            var tk = new TaiKhoan
            {
                MaTaiKhoan = Guid.NewGuid().ToString(),
                TenDangNhap = request.TenDangNhap,
                Email = request.Email,
                HoTen = request.HoTen,
                SoDienThoai = request.SoDienThoai,
                DiaChi = request.DiaChi,
                VaiTro = "user",
            };

            tk.MatKhau = _hasher.HashPassword(tk, request.MatKhau);

            await _repo.AddAsync(tk);
            await _repo.SavechangeAsync();
            return (true, "Đăng ký thành công");
            
        }
        private string GenerateToken(TaiKhoan user)
        {
            // 1. Tạo Key bảo mật từ chuỗi bí mật trong appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            // 2. Tạo thuật toán ký (Signature)
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. Tạo các Claim (Thông tin nhét vào trong Token)
            var claims = new[]
            {
                new Claim("MaTaiKhoan", user.MaTaiKhoan),
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, user.VaiTro ?? "user"), // Quan trọng để phân quyền
                new Claim("HoTen", user.HoTen ?? "")
            };

            // 4. Cấu hình Token (Ai phát hành, Hết hạn khi nào...)
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Token sống 2 tiếng
                signingCredentials: credentials);

            // 5. Viết Token ra chuỗi string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<(bool, string, DangNhapResponse)> DangNhapAsync(DangNhapRequest request)
        {
            var exist = await _repo.GetByTenDangNhapAsync(request.TenDangNhap);
            if(exist == null)
            {
                return (false, "Tên đăng nhập hoặc mật khẩu không chính xác",  null);
            }
            var result = _hasher.VerifyHashedPassword(exist, exist.MatKhau, request.MatKhau);
            if(result == PasswordVerificationResult.Failed)
            {
                return (false, "Tên đăng nhập hoặc mật khẩu không chính xác", null);
            }
            var tokenString = GenerateToken(exist);
            return (true,"Dăng nhập thành công",new DangNhapResponse
            {
                MaTaiKhoan = exist.MaTaiKhoan,
                TenDangNhap = exist.TenDangNhap,
                Email = exist.Email,
                VaiTro = exist.VaiTro,
                Token = tokenString,
            });
        }
        public async Task<IEnumerable<TaiKhoanResponse>> GetAllTaiKhoan()
        {
            var data = await _repo.GetAllTaiKhoan();
            return data.Select(TaiKhoanBuilder.ToResponse);
        }

        public async Task<TaiKhoanResponse?> GetTaiKhoanById(string id)
        {
            var tk = await _repo.GetByMaTaiKhoan(id);
            return tk == null ? null : TaiKhoanBuilder.ToResponse(tk);
        }

        public async Task<(bool, string, TaiKhoanResponse?)> UpdateProfile(string id, UpdateProfileRequest request)
        {
            var tk = await _repo.GetByMaTaiKhoan(id);
            if (tk == null) return (false, "Tài khoản không tồn tại", null);

            tk.HoTen = request.HoTen ?? tk.HoTen;
            tk.SoDienThoai = request.SoDienThoai ?? tk.SoDienThoai;
            tk.DiaChi = request.DiaChi ?? tk.DiaChi;
            tk.Email = request.Email ?? tk.Email;

            await _repo.UpdateAsync(tk);
            return (true, "Cập nhật thông tin thành công", TaiKhoanBuilder.ToResponse(tk));
        }

        public async Task<(bool, string, TaiKhoanResponse?)> ChangePassword(string id, ChangePasswordRequest request)
        {
            var tk = await _repo.GetByMaTaiKhoan(id);
            if (tk == null) return (false, "Tài khoản không tồn tại", null);

            var verify = _hasher.VerifyHashedPassword(tk, tk.MatKhau, request.MatKhauCu);
            if (verify == PasswordVerificationResult.Failed)
            {
                return (false, "Mật khẩu cũ không chính xác", null);
            }

            tk.MatKhau = _hasher.HashPassword(tk, request.MatKhauMoi);

            await _repo.UpdateAsync(tk);
            return (true, "Đổi mật khẩu thành công", TaiKhoanBuilder.ToResponse(tk));
        }

        public async Task<(bool, string, TaiKhoanResponse?)> UpdateRole(string id, string newRole)
        {
            var tk = await _repo.GetByMaTaiKhoan(id);
            if (tk == null) return (false, "Tài khoản không tồn tại", null);

            if (newRole.ToLower() != "admin" && newRole.ToLower() != "user")
                return (false, "Role không hợp lệ (chỉ chấp nhận 'admin' hoặc 'user')", null);

            tk.VaiTro = newRole.ToLower();
            await _repo.UpdateAsync(tk);
            return (true, $"Đã cập nhật quyền thành {newRole}", TaiKhoanBuilder.ToResponse(tk));
        }

        public async Task<(bool, string)> DeleteTaiKhoan(string id)
        {
            var result = await _repo.DeleteOrLockTaiKhoan(id);
            if (!result)
            {
                return (false, "Không thể xóa (Tài khoản không tồn tại hoặc ĐÃ CÓ ĐƠN HÀNG)");
            }
            return (true, "Xóa tài khoản thành công");
        }
    }
}
