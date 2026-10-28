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
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiService _service;
        public KhuyenMaiController(IKhuyenMaiService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getallkm()
        {
            var data = await _service.GetAllKM();
            return Ok(APIResponse<IEnumerable<KhuyenMaiResponse>>.OK("Lấy danh sách khuyến mãi thành công", data));
        }
        [HttpGet]
        [Route("by-makm/{makm}")]
        public async Task<IActionResult> getkmbymakm(string makm)
        {
            var data = await _service.GetKMByMaKM(makm);
            if (data == null)
            {
                return NotFound(APIResponse<KhuyenMaiResponse>.Fail($"Không tìm thấy khuyến mãi có mã {makm}"));
            }
            return Ok(APIResponse<KhuyenMaiResponse>.OK("Đã tìm thấy khuyến mãi", data));
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> postkmasync(KhuyenMaiRequest request)
        {
            var data = await _service.PostKM(request);

            if (!data.success)
            {
                return BadRequest(APIResponse<KhuyenMaiResponse>.Fail(data.message));
            }

            return Ok(APIResponse<KhuyenMaiResponse>.OK(data.message, data.response));
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> putkmasync(KhuyenMaiRequest request)
        {
            var data = await _service.UpdateKM(request);

            if (!data.success)
            {
                return BadRequest(APIResponse<KhuyenMaiResponse>.Fail(data.message));
            }

            return Ok(APIResponse<KhuyenMaiResponse>.OK(data.message, data.response));
        }
        [HttpDelete]
        [Route("{makm}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> deletekmasync(string makm)
        {
            var data = await _service.DeleteKM(makm);
            if (!data.success)
            {
                return NotFound(data.message);
            }
            return Ok(data.message);
        }
        [HttpGet]
        [Route("chitiet/by-makm/{makm}")]
        public async Task<IActionResult> getchitietbymakm(string makm)
        {
            var data = await _service.GetChiTietKMByMaKM(makm);
            if (data == null)
            {
                return NotFound(APIResponse<ChiTietKhuyenMaiResponse>.Fail("Không tồn tại mã khuyến mãi này"));
            }
            return Ok(APIResponse<IEnumerable<ChiTietKhuyenMaiResponse>>.OK("Danh sách chi tiết theo khuyến mãi", data));
        }
        [HttpGet]
        [Route("chitiet/by-masp/{masp}")]
        public async Task<IActionResult> getchitietbymasp(string masp)
        {
            var data = await _service.GetChiTietKMByMaSP(masp);
            if(data == null)
            {
                return NotFound(APIResponse<ChiTietKhuyenMaiResponse>.Fail("Không tồn tại mã sản phẩm này"));
            }
            return Ok(APIResponse<IEnumerable<ChiTietKhuyenMaiResponse>>.OK("Danh sách chi tiết theo sản phẩm", data));
        }
        [HttpPost]
        [Route("chitiet")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> postchitietasync(ChiTietKhuyenMaiRequest request)
        {
            var data = await _service.PostChiTietKM(request);

            if (!data.success)
            {
                return BadRequest(APIResponse<ChiTietKhuyenMaiResponse>.Fail(data.message));
            }

            return Ok(APIResponse<ChiTietKhuyenMaiResponse>.OK(data.message, data.response));
        }
        [HttpPut]
        [Route("chitiet")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> putchitietasync(ChiTietKhuyenMaiRequest request)
        {
            var (success, message, response) = await _service.UpdateChiTietKM(request);

            if (!success)
            {
                return BadRequest(APIResponse<ChiTietKhuyenMaiResponse>.Fail(message));
            }

            return Ok(APIResponse<ChiTietKhuyenMaiResponse>.OK(message, response));
        }
        [HttpDelete]
        [Route("chitiet/{makm}/{masp}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> deletechitietasync(string makm, string masp)
        {
            var data = await _service.DeleteChiTietKM(makm, masp);

            if (!data.success)
            {
                return NotFound(data.message);
            }

            return Ok(data.message);
        }
    }
}
