using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;
using DACN_H_P.Service;
using DACN_H_P.Service.Impl;
using DACN_H_P.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DACN_H_P.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITaiKhoanService _service;

        public AuthenticationController(ITaiKhoanService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> loginasync([FromBody] DangNhapRequest request)
        {
            var result = await _service.DangNhapAsync(request);
            if(!result.success)
            {
                return BadRequest(APIResponse<DangNhapResponse>.Fail(result.message));
            }
            return Ok(APIResponse<DangNhapResponse>.OK(result.message, result.response));
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> registerasync([FromBody] DangKyRequest request)
        {
            var result = await _service.DangKyAsync(request);
            if(!result.success)
            {
                return BadRequest(APIResponse<string>.Fail(result.message));
            }
            return Ok(APIResponse<string>.OK(result.message,null));
        }
    }
}
