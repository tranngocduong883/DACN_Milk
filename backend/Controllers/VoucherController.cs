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
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _service;

        public VoucherController(IVoucherService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getallasync()
        {
            var data = await _service.GetAllVouchers();
            return Ok(APIResponse<IEnumerable<VoucherResponse>>.OK("Lấy thành công danh sách voucher", data));
        }

        [HttpGet]
        [Route("{maVoucher}")]
        public async Task<IActionResult> getbymavoucherasync(string maVoucher)
        {
            var data = await _service.GetVoucherByCode(maVoucher);
            if (data == null)
            {
                return NotFound(APIResponse<VoucherResponse>.Fail("Không có mã voucher này"));
            }
            return Ok(APIResponse<VoucherResponse>.OK("Lấy thông tin voucher thành công", data));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(VoucherRequest request)
        {
            var result = await _service.CreateVoucher(request);
            if (!result.Success)
            {
                return BadRequest(APIResponse<VoucherResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<VoucherResponse>.OK(result.Message,result.response));
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(VoucherRequest request)
        {
            var result = await _service.UpdateVoucher(request);
            if (!result.Success)
            {
                return BadRequest(APIResponse<VoucherResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<VoucherResponse>.OK(result.Message, result.response));
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string maVoucher)
        {
            var result = await _service.DeleteVoucher(maVoucher);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet]
        [Route("available/{maTk}")]
        public async Task<IActionResult> getavaiablevouchers(string maTk)
        {
            var data = await _service.GetVouchersForUser(maTk);

            return Ok(APIResponse<IEnumerable<VoucherResponse>>.OK("Lấy thành công danh sách voucher theo tài khoản", data));
        }
    }
}
