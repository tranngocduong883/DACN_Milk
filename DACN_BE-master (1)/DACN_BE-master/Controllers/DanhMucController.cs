using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Repository.Impl;
using DACN_H_P.Service;
using DACN_H_P.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DACN_H_P.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        private readonly IDanhMucService _service;

        public DanhMucController(IDanhMucService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> getallasync()
        {
            var data = await _service.GetAllAsync();
            return Ok(APIResponse<IEnumerable<DanhMucResponse>>.OK("Lấy danh sách thành công" , data));
        }
        [HttpGet]
        [Route("by-madm/{madm}")]
        public async Task<IActionResult> getbymadm(string madm)
        {
            var data = await _service.GetByMaDMAsync(madm);
            if(data == null)
            {
                return NotFound(APIResponse<DanhMucResponse>.Fail($"Không tìm thấy danh mục có mã {madm}"));
            }
            return Ok(APIResponse<DanhMucResponse>.OK($"Đã tìm thấy danh mục có mã {madm}", data));
        }
        [HttpGet]
        [Route("by-tendm/{tendm}")]
        public async Task<IActionResult> getbytendm(string tendm)
        {
            var data = await _service.GetByTenDMAsync(tendm);
            if(data == null)
            {
                return NotFound(APIResponse<IEnumerable<DanhMucResponse>>.Fail("Không có danh mục nào"));
            }
            return Ok(APIResponse<IEnumerable<DanhMucResponse>>.OK("danh sách danh mục", data));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> postasync(DanhMucRequest request)
        {
            var data = await _service.PostAsync(request);
            if (!data.success)
            {
                return BadRequest(APIResponse<DanhMucResponse>.Fail(data.message));
            }
            return Ok(APIResponse<DanhMucResponse>.OK(data.message, data.response));
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> editasync(DanhMucRequest request)
        {
            var data = await _service.EditAsync(request);
            if (!data.success)
            {
                return BadRequest(APIResponse<DanhMucResponse>.Fail(data.message));
            }
            return Ok(APIResponse<DanhMucResponse>.OK(data.message, data.response));
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> deleteasync(string madm)
        {
            var data = await _service.DeleteAsync(madm);
            if (!data.succes)
            {
                return NotFound(data.message);
            }
            return Ok(data.message);
        }
        
    }
}
