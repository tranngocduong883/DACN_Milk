
using DACN_H_P.Helper;
using DACN_H_P.Model;
using DACN_H_P.Repository;
using DACN_H_P.Repository.Impl;
using DACN_H_P.Service;
using DACN_H_P.Service.Impl;
using DACN_H_P.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DACN_H_P
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
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

            builder.Services.AddDbContext<DacnHContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));
            builder.Services.AddSingleton<CloudinaryService>();

            builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
            builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();

            builder.Services.AddScoped<IDanhMucRepository, DanhMucRepository>();
            builder.Services.AddScoped<IDanhMucService, DanhMucService>();

            builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
            builder.Services.AddScoped<ISanPhamService, SanPhamService>();

            builder.Services.AddScoped<IAnhRepository, AnhRepository>();
            builder.Services.AddScoped<IAnhService, AnhService>();

            builder.Services.AddScoped<IGioHangRepository, GioHangRepository>();
            builder.Services.AddScoped<IGioHangService, GioHangService>();

            builder.Services.AddScoped<IKhuyenMaiRepository, KhuyenMaiRepository>();
            builder.Services.AddScoped<IKhuyenMaiService, KhuyenMaiService>();

            builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
            builder.Services.AddScoped<IVoucherService, VoucherService>();

            builder.Services.AddScoped<IDonHangRepositoy, DonHangRepository>();
            builder.Services.AddScoped<IDonHangService, DonHangService>();

            builder.Services.AddScoped<ICuaHangRepository, CuaHangRepository>();
            builder.Services.AddScoped<ICuaHangService, CuaHangService>();

            builder.Services.AddScoped<IDanhGiaRepository, DanhGiaRepository>();
            builder.Services.AddScoped<IDanhGiaService, DanhGiaService>();

            //AddJWT
            builder.Services.AddAuthentication(options =>
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
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
