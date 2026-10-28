using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Service;
using DACN_H_P.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DACN_H_P.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly ITaiKhoanService _service;
        public TaiKhoanController(ITaiKhoanService service)
        {
            _service = service;
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> getallasync()
        {
            var data = await _service.GetAllTaiKhoan();
            return Ok(APIResponse<IEnumerable<TaiKhoanResponse>>.OK("Lấy danh sách tài khoản thành công", data));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> getbymatkasync(string id)
        {
            // Admin xem được mọi tài khoản, người dùng chỉ xem được tài khoản của mình
            if (!User.IsInRole("admin") && User.FindFirst("MaTaiKhoan")?.Value != id)
            {
                return Forbid();
            }
            var data = await _service.GetTaiKhoanById(id);
            if (data == null)
            {
                return NotFound(APIResponse<TaiKhoanResponse>.Fail("Không tìm thấy tài khoản"));
            }
            return Ok(APIResponse<TaiKhoanResponse>.OK("Đã tìm thấy tài khoản",data));
        }

        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdateProfile(string id, [FromBody] UpdateProfileRequest req)
        {
            var result = await _service.UpdateProfile(id, req);
            if (!result.Success)
                return BadRequest(APIResponse<TaiKhoanResponse>.Fail(result.Message));

            return Ok(APIResponse<TaiKhoanResponse>.OK(result.Message, result.response));
        }

        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] ChangePasswordRequest req)
        {
            var result = await _service.ChangePassword(id, req);
            if (!result.Success)
                return BadRequest(APIResponse<TaiKhoanResponse>.Fail(result.Message));

            return Ok(APIResponse<TaiKhoanResponse>.OK(result.Message, result.response));
        }

        [HttpPut("admin/role/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleRequest req)
        {
            var result = await _service.UpdateRole(id, req.VaiTroMoi);
            if (!result.Success)
                return BadRequest(APIResponse<TaiKhoanResponse>.Fail(result.Message));

            return Ok(APIResponse<TaiKhoanResponse>.OK(result.Message, result.response));
        }

        [HttpDelete("admin/{id}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteTaiKhoan(id);
            if (!result.Success)
                return BadRequest(APIResponse<string>.Fail(result.Message));

            return Ok(APIResponse<string>.OK(result.Message, null));
        }
    }
}
