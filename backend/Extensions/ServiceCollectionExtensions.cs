using DACN_H_P.Helper;
using DACN_H_P.Model;
using DACN_H_P.Repository;
using DACN_H_P.Repository.Impl;
using DACN_H_P.Service;
using DACN_H_P.Service.Impl;
using DACN_H_P.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DACN_H_P.Extensions
{
    // Gom các bước đăng ký dịch vụ để Program.cs ngắn gọn, dễ đọc
    public static class ServiceCollectionExtensions
    {
        // DbContext, Cloudinary và các cặp Repository/Service
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DacnHContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.Configure<CloudinarySetting>(configuration.GetSection("CloudinarySetting"));
            services.AddSingleton<CloudinaryService>();

            services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
            services.AddScoped<ITaiKhoanService, TaiKhoanService>();

            services.AddScoped<IDanhMucRepository, DanhMucRepository>();
            services.AddScoped<IDanhMucService, DanhMucService>();

            services.AddScoped<ISanPhamRepository, SanPhamRepository>();
            services.AddScoped<ISanPhamService, SanPhamService>();

            services.AddScoped<IAnhRepository, AnhRepository>();
            services.AddScoped<IAnhService, AnhService>();

            services.AddScoped<IGioHangRepository, GioHangRepository>();
            services.AddScoped<IGioHangService, GioHangService>();

            services.AddScoped<IKhuyenMaiRepository, KhuyenMaiRepository>();
            services.AddScoped<IKhuyenMaiService, KhuyenMaiService>();

            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IVoucherService, VoucherService>();

            services.AddScoped<IDonHangRepositoy, DonHangRepository>();
            services.AddScoped<IDonHangService, DonHangService>();

            services.AddScoped<ICuaHangRepository, CuaHangRepository>();
            services.AddScoped<ICuaHangService, CuaHangService>();

            services.AddScoped<IDanhGiaRepository, DanhGiaRepository>();
            services.AddScoped<IDanhGiaService, DanhGiaService>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

            return services;
        }

        // Swagger có nút Authorize để gửi kèm JWT khi gọi thử API
        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập token vào đây"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
