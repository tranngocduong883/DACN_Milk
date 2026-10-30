using CloudinaryDotNet;
using DACN_H_P.Helper;
using DACN_H_P.Utils;
using Microsoft.Extensions.Options;
using CloudinaryDotNet.Actions;
namespace DACN_H_P.Helper
{
    public class CloudinaryService
    {
        private readonly Cloudinary? _cloudinary;

        public CloudinaryService(IOptions<CloudinarySetting> config)
        {
            // Khóa Cloudinary đặt trong user-secrets / biến môi trường, không commit lên git.
            // Chưa cấu hình thì vẫn chạy được, chỉ bỏ qua việc upload ảnh.
            if (string.IsNullOrWhiteSpace(config.Value.CloudName) || string.IsNullOrWhiteSpace(config.Value.ApiSecret))
            {
                return;
            }
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if(_cloudinary == null || file == null || file.Length == 0)
            {
                return null;
            }
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "DACN_IMAGES"
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }
    }
}
