using DACN_H_P.Extensions;
using DACN_H_P.Middleware;
using DACN_H_P.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DACN_H_P
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Trả lỗi validate theo cùng định dạng APIResponse, lấy thông báo lỗi đầu tiên
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var message = context.ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .FirstOrDefault(m => !string.IsNullOrWhiteSpace(m)) ?? "Dữ liệu không hợp lệ";
                        return new BadRequestObjectResult(APIResponse<object>.Fail(message));
                    };
                });

            builder.Services.AddSwaggerWithJwt();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

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
