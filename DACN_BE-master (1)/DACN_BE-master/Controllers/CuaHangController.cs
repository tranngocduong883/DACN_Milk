using DACN_H_P.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DACN_H_P.Utils;
using DACN_H_P.Dtos.Response;

namespace DACN_H_P.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuaHangController : ControllerBase
    {
        private readonly ICuaHangService _service;
        public CuaHangController(ICuaHangService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getdschasync()
        {
            var result = await _service.GetDSCH();
            return Ok(APIResponse<IEnumerable<CuaHangResponse>>.OK("Lấy danh sách thành công", result));
        }
    }
}
