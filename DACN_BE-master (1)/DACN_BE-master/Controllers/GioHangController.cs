using DACN_H_P.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DACN_H_P.Utils;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Dtos.Request;
using Microsoft.AspNetCore.Authorization;

namespace DACN_H_P.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GioHangController : ControllerBase
    {
        private readonly IGioHangService _service;

        public GioHangController(IGioHangService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getgiohangasync(string matk)
        {
            var data = await _service.GetGioHangByMaTaiKhoanAsync(matk);
            if (!data.success)
            {
                return NotFound(APIResponse<GioHangResponse>.Fail(data.message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.message, data.response));
        }
        [HttpPost]
        [Route("addOrupdate/{matk}")]
        public async Task<IActionResult> addorupdateasync([FromRoute] string matk, [FromBody] ChiTietGioHangRequest request)
        {
            var data = await _service.AddOrUpdateGioHang(matk, request);
            if (!data.success)
            {
                return NotFound(APIResponse<GioHangResponse>.Fail(data.message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.message, data.response));
        }
        [HttpPost]
        [Route("decreaseItem/{matk}")]
        public async Task<IActionResult> decreaseitemasync([FromRoute] string matk, [FromBody] ChiTietGioHangRequest request)
        {
            var data = await _service.DecreaseItemGioHang(matk, request);
            if (!data.success)
            {
                return NotFound(APIResponse<GioHangResponse>.Fail(data.message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.message, data.response));
        }
        [HttpPost]
        [Route("updateItem/{matk}")]
        public async Task<IActionResult> updateitemasync([FromRoute] string matk, [FromBody] ChiTietGioHangRequest request)
        {
            var data = await _service.UpdateItemGioHang(matk, request);
            if (!data.success)
            {
                return NotFound(APIResponse<GioHangResponse>.Fail(data.message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.message, data.response));
        }
        [HttpDelete]
        [Route("removeItem/{matk}/{masp}")]
        public async Task<IActionResult> removeitemasync([FromRoute] string matk, [FromRoute] string masp)
        {
            var data = await _service.RemoveItemAsync(matk, masp);
            if (!data.success)
            {
                return NotFound(APIResponse<GioHangResponse>.Fail(data.message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.message, data.response));
        }
        [HttpDelete]
        [Route("removeAll/{matk}")]
        public async Task<IActionResult> removeallasync(string matk)
        {
            var data = await _service.RemoveAllGioHang(matk);
            if (!data.success)
            {
                return NotFound(APIResponse<GioHangResponse>.Fail(data.message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.message, data.response));
        }
    }
}
