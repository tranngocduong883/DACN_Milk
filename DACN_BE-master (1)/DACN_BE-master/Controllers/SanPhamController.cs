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
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamService _service;
        public SanPhamController(ISanPhamService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(APIResponse<IEnumerable<SanPhamResponse>>.OK("Lấy danh sách thành công", result));
        }
        [HttpGet]
        [Route("by-masp/{masp}")]
        public async Task<IActionResult> GetByMaSP(string masp)
        {
            var data = await _service.GetByMaSP(masp);
            if (data == null)
            {
                return NotFound(APIResponse<SanPhamResponse>.Fail($"Không tìm thấy sản phẩm có mã {masp}"));
            }
            return Ok(APIResponse<SanPhamResponse>.OK($"Đã tìm thấy sản phẩm có mã {masp}", data));
        }
        [HttpGet]
        [Route("by-tensp/{tensp}")]
        public async Task<IActionResult> GetByTenSP(string tensp)
        {
            var data = await _service.GetByTenSP(tensp);
            if (data == null)
            {
                return NotFound(APIResponse<IEnumerable<SanPhamResponse>>.Fail("Không có sản phẩm nào"));
            }
            return Ok(APIResponse<IEnumerable<SanPhamResponse>>.OK("Danh sách sản phẩm", data));
        }
        [HttpGet]
        [Route("by-madm/{madm}")]
        public async Task<IActionResult> GetByMaDM(string madm)
        {
            var data = await _service.GetByMaDM(madm);
            if (data == null)
            {
                return NotFound(APIResponse<IEnumerable<SanPhamResponse>>.Fail("Không có danh mục sản phẩm này"));
            }
            return Ok(APIResponse<IEnumerable<SanPhamResponse>>.OK("Danh sách sản phẩm", data));
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostAsync([FromForm] SanPhamRequest request)
        {
            var data = await _service.PostAsync(request);
            if (!data.success)
            {
                return BadRequest(APIResponse<SanPhamResponse>.Fail(data.message));
            }
            return Ok(APIResponse<SanPhamResponse>.OK(data.message, data.response));
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutAsync([FromForm] SanPhamRequest request)
        {
            var data = await _service.PutAsync(request);
            if (!data.success)
            {
                return NotFound(APIResponse<SanPhamResponse>.Fail(data.message));
            }
            return Ok(APIResponse<SanPhamResponse>.OK(data.message, data.response));
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAsync(string masp)
        {
            var data = await _service.DeleteAsync(masp);
            if (!data.success)
            {
                return NotFound(data.message);
            }
            return Ok(data.message);
        }
    }
}
