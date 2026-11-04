# NutriMilk – Website bán sữa

Website thương mại điện tử bán sữa cho thương hiệu NutriMilk, gồm trang mua hàng cho khách hàng và trang quản trị. Đây là đồ án chuyên ngành (IT6133) – Trường Công nghệ Thông tin và Truyền thông, Đại học Công nghiệp Hà Nội.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)
![React](https://img.shields.io/badge/React-19-61DAFB)
![TypeScript](https://img.shields.io/badge/TypeScript-5.8-3178C6)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-CC2927)
![Postman](https://img.shields.io/badge/API%20tests-Postman%20%2F%20Newman-FF6C37)

## Chức năng

**Khách hàng:** đăng ký, đăng nhập, xem sản phẩm theo danh mục, tìm kiếm, xem chi tiết, giỏ hàng, thanh toán có voucher, theo dõi đơn hàng, xem lịch sử mua hàng, đánh giá sản phẩm, quản lý thông tin cá nhân.

**Quản trị viên:** quản lý sản phẩm, danh mục, voucher, khuyến mãi, đơn hàng, người dùng, đánh giá; thống kê doanh thu.

## Công nghệ

| Phần | Công nghệ |
|---|---|
| Backend | ASP.NET Core 8 Web API, Entity Framework Core, JWT, Swagger |
| Frontend | React 19, TypeScript, Vite, React Router, Tailwind CSS |
| Cơ sở dữ liệu | SQL Server 2019 |
| Kiểm thử | Manual test case, Postman / Newman |
| Triển khai | Docker Compose |

## Cấu trúc thư mục

```
├── backend/              ASP.NET Core Web API (Controller – Service – Repository)
├── frontend/             React + TypeScript
├── database/             Script tạo cơ sở dữ liệu và dữ liệu mẫu
├── docs/                 Báo cáo đồ án, biểu đồ use case, ERD
├── test/                 Test case, báo cáo kiểm thử, bug report, test API tự động
└── docker-compose.yml    Chạy SQL Server và backend bằng Docker
```

## Cài đặt và chạy

### Yêu cầu

- .NET SDK 8
- Node.js 20 trở lên
- SQL Server 2019 trở lên (hoặc Docker)

### 1. Tạo cơ sở dữ liệu

Chạy lần lượt hai script trong SQL Server Management Studio hoặc DBeaver:

1. `database/init-db.sql` – tạo database `DACN_H`, các bảng và tài khoản quản trị mẫu
2. `database/seed-products.sql` – thêm sản phẩm mẫu

### 2. Chạy backend

Sửa chuỗi kết nối `DefaultConnection` trong `backend/appsettings.json` cho đúng SQL Server trên máy, sau đó:

```bash
cd backend
dotnet run
```

API chạy tại `http://localhost:5014`, tài liệu Swagger tại `http://localhost:5014/swagger`.

### 3. Chạy frontend

```bash
cd frontend
npm install
npm run dev
```

Mở `http://localhost:5173`. Vite tự chuyển các request `/api` sang backend ở cổng 5014.

### Chạy bằng Docker

```bash
docker compose up -d --build
```

SQL Server không tự chạy script khởi tạo, nên lần đầu cần tạo database bằng `sqlcmd` trong container (thư mục `database/` đã được gắn vào `/database`):

```bash
docker exec -it sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Password123@" -C -i /database/init-db.sql
docker exec -it sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Password123@" -C -i /database/seed-products.sql
```

Với image SQL Server cũ, đường dẫn là `/opt/mssql-tools/bin/sqlcmd` và bỏ tham số `-C`. Backend chạy tại `http://localhost:7000`; khi chạy frontend, đặt `API_TARGET=http://localhost:7000` trong `frontend/.env.local`.

## Kiểm thử

- **42 test case thủ công** cho 10 chức năng, 100% Pass – [test-cases.md](test/test-cases.md), [báo cáo](test/test-report.md)
- **Test API tự động** bằng Postman: 14 request, 28 assertion, gồm kiểm thử giá trị biên và phân quyền – [hướng dẫn chạy](test/README.md)
- **Bug report:** [BUG-01](test/bug-reports.md) – đăng ký trùng email trả về lỗi 500, phát hiện bằng test API tự động, đã sửa và retest

```bash
npx newman run test/api/NutriMilk.postman_collection.json --env-var baseUrl=http://localhost:5014
```

## Tài liệu

Báo cáo đồ án đầy đủ (khảo sát, đặc tả 21 use case, biểu đồ trình tự, ERD, wireframe, kế hoạch kiểm thử): [docs/](docs/README.md)

## Tác giả

**Trần Ngọc Dương** – Sinh viên ngành Hệ thống thông tin, Đại học Công nghiệp Hà Nội

- GitHub: [@tranngocduong883](https://github.com/tranngocduong883)
- Email: tranngocduong230805@gmail.com
