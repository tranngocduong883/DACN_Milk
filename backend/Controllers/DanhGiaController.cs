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
    public class DanhGiaController : ControllerBase
    {
        private readonly IDanhGiaService _service;

        public DanhGiaController(IDanhGiaService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] TaoDanhGiaRequest request)
        {
            var result = await _service.TaoDanhGia(request);
            if (!result.Success)
            {
                // Trả về lỗi chuẩn style của bạn
                return BadRequest(APIResponse<DanhGiaResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<DanhGiaResponse>.OK(result.Message, result.Data));
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] SuaDanhGiaRequest request)
        {
            var result = await _service.SuaDanhGia(request);
            if (!result.Success)
            {
                return BadRequest(APIResponse<DanhGiaResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<DanhGiaResponse>.OK(result.Message, result.Data));
        }

        // --- PUBLIC ROUTES ---

        [HttpGet("product/{maSp}")]
        public async Task<IActionResult> GetByProduct(string maSp)
        {
            var data = await _service.GetByProduct(maSp);
            // Get luôn thành công, data rỗng thì trả về mảng rỗng
            return Ok(APIResponse<IEnumerable<DanhGiaResponse>>.OK("Lấy danh sách đánh giá thành công", data));
        }

        // --- ADMIN ROUTES ---

        [HttpGet("admin/list")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminGetList()
        {
            var data = await _service.AdminGetAll();
            return Ok(APIResponse<IEnumerable<DanhGiaResponse>>.OK("Lấy toàn bộ đánh giá thành công", data));
        }

        [HttpPut("admin/approve/{maDanhGia}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminApprove(string maDanhGia, [FromBody] DuyetDanhGiaRequest request)
        {
            var result = await _service.DuyetDanhGia(maDanhGia, request.TrangThai);
            if (!result.Success)
            {
                return BadRequest(APIResponse<DanhGiaResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<DanhGiaResponse>.OK(result.Message, result.Data));
        }

        [HttpDelete("admin/delete/{maDanhGia}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminDelete(string maDanhGia)
        {
            var result = await _service.XoaDanhGia(maDanhGia);
            if (!result.Success)
            {
                // Delete lỗi thì trả về string
                return BadRequest(APIResponse<string>.Fail(result.Message));
            }
            // Thành công trả về null data
            return Ok(APIResponse<string>.OK(result.Message, null));
        }
    }
}
