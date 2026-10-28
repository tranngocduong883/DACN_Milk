using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Service;
using DACN_H_P.Utils;
using DACN_H_P.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DACN_H_P.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnhController : ControllerBase
    {
        private readonly IAnhService _service;

        public AnhController(IAnhService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getallasync()
        {
            var result = await _service.GetAllAnh();
            return Ok(APIResponse<IEnumerable<AnhResponse>>.OK("Lấy danh sách ảnh thành công", result));
        }
        [HttpGet]
        [Route("by-masp/{masp}")]
        public async Task<IActionResult> getbymaspasync(string masp)
        {
            var result = await _service.GetByMaSP(masp);
            if(result == null)
            {
                return NotFound(APIResponse<IEnumerable<AnhResponse>>.Fail("Không có mã sản phẩm này"));
            }
            return Ok(APIResponse<IEnumerable<AnhResponse>>.OK("Lấy ảnh theo mã sản phẩm thành công", result));
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> postasync([FromForm] AnhRequest request)
        {
           var result = await _service.PostAsync(request);
            if (!result.success)
            {
                return NotFound(APIResponse<IEnumerable<AnhResponse>>.Fail(result.message));
            }
            return Ok(APIResponse<IEnumerable<AnhResponse>>.OK(result.message, result.response));
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> deleteasync(string maanh)
        {
            var result = await _service.DeleteAsync(maanh);
            if (!result.success)
            {
                return NotFound(result.message);
            }
            return Ok(result.message);
        }
    }
}
