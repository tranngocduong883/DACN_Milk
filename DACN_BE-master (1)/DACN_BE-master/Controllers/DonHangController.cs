using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Service;
using DACN_H_P.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DACN_H_P.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangService _service;

        public DonHangController(IDonHangService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> createdonhangasync(DatHangRequest request)
        {
            var result = await _service.CreateDonHang(request);
            if (!result.success)
            {
                return BadRequest(APIResponse<DonHangResponse>.Fail(result.message));
            }
            return Ok(APIResponse<DonHangResponse>.OK( result.message, result.response));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> getallasync()
        {
            var data = await _service.GetAllDonHang();
            return Ok(APIResponse<IEnumerable<DonHangResponse>>.OK("Lấy danh sách đơn hàng thành công", data));
        }

        [HttpGet]
        [Route("{maDonHang}")]
        public async Task<IActionResult> getbymadonhangasync(string maDonHang)
        {
            var data = await _service.GetDonHangByMaDH(maDonHang);
            if (data == null)
            {
                return NotFound(APIResponse<DonHangResponse>.Fail($"Không tìm thấy đơn hàng có mã {maDonHang}"));
            }
            return Ok(APIResponse<DonHangResponse>.OK("Tìm thấy đơn hàng", data));
        }

        [HttpGet]
        [Route("user/{maTk}")]
        public async Task<IActionResult> getbymatkasync(string maTk)
        {
            var data = await _service.GetDonHangByMaTK(maTk);
            if (data == null)
            {
                return NotFound(APIResponse<DonHangResponse>.Fail("Không có tài khoản này"));
            }
            return Ok(APIResponse<IEnumerable<DonHangResponse>>.OK("Lấy lịch sử đơn hàng thành công", data));
        }

        [HttpPut]
        [Route("trangthai/{maDonHang}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> updatestatusasync(string maDonHang, [FromBody] UpdateTrangThaiDonRequest request)
        {
            var result = await _service.UpdateTrangThaiDonHang(maDonHang, request.TrangThaiMoi);
            if (!result.Success)
            {
                return BadRequest(APIResponse<DonHangResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<DonHangResponse>.OK(result.Message, result.response));
        }

        [HttpPut]
        [Route("huydon/{maDonHang}")]
        public async Task<IActionResult> canceldonhangasync(string maDonHang, [FromBody] HuyDonRequest request)
        {
            var result = await _service.HuyDonHang(maDonHang, request.MaTaiKhoan);
            if (!result.Success)
            {
                return BadRequest(APIResponse<DonHangResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<DonHangResponse>.OK(result.Message, new DonHangResponse()));
        }

        [HttpPut]
        [Route("diachi/{maDonHang}")]
        public async Task<IActionResult> updatediachiasync(string maDonHang, [FromBody] UpdateDiaChiRequest request)
        {
            var result = await _service.UpdateDiaChiGiaoHang(maDonHang, request.MaTaiKhoan, request.DiaChiMoi);
            if (!result.Success)
            {
                return BadRequest(APIResponse<DonHangResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<DonHangResponse>.OK(result.Message, result.response));
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> deleteasync(string maDH)
        {
            var result = await _service.DeleteDonHang(maDH);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
