using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Model;

public partial class DacnHContext : DbContext
{
    public DacnHContext()
    {
    }

    public DacnHContext(DbContextOptions<DacnHContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnhSp> AnhSps { get; set; }

    public virtual DbSet<ChiTietDh> ChiTietDhs { get; set; }

    public virtual DbSet<ChiTietGh> ChiTietGhs { get; set; }

    public virtual DbSet<ChiTietKm> ChiTietKms { get; set; }

    public virtual DbSet<CuaHang> CuaHangs { get; set; }

    public virtual DbSet<DanhGiaSp> DanhGiaSps { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<PhuongThucTt> PhuongThucTts { get; set; }

    public virtual DbSet<PhuongThucVc> PhuongThucVcs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Chỉ dùng chuỗi mặc định khi chưa cấu hình qua DI (appsettings / biến môi trường)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=REDAMANCY;Initial Catalog=DACN_H;Integrated Security=True;Trust Server Certificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnhSp>(entity =>
        {
            entity.HasKey(e => e.MaAnh).HasName("PK__ANH_SP__184D7736CD4D9D8B");

            entity.ToTable("ANH_SP");

            entity.Property(e => e.MaAnh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maAnh");
            entity.Property(e => e.DuongDan)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("duongDan");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.AnhSps)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnhSP_MaSP");
        });

        modelBuilder.Entity<ChiTietDh>(entity =>
        {
            entity.HasKey(e => new { e.MaDonHang, e.MaSp }).HasName("PK__CHI_TIET__30BF1FBE1335B6C2");

            entity.ToTable("CHI_TIET_DH");

            entity.Property(e => e.MaDonHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maDonHang");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");
            entity.Property(e => e.DonGiaLucDat)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("donGiaLucDat");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDhs)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maDon__72C60C4A");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietDhs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET_D__maSP__73BA3083");
        });

        modelBuilder.Entity<ChiTietGh>(entity =>
        {
            entity.HasKey(e => new { e.MaGioHang, e.MaSp }).HasName("PK__CHI_TIET__9BD4F5A46E5E4423");

            entity.ToTable("CHI_TIET_GH");

            entity.Property(e => e.MaGioHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maGioHang");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ChiTietGhs)
                .HasForeignKey(d => d.MaGioHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maGio__68487DD7");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietGhs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET_G__maSP__693CA210");
        });

        modelBuilder.Entity<ChiTietKm>(entity =>
        {
            entity.HasKey(e => new { e.MaKhuyenMai, e.MaSp }).HasName("PK__CHI_TIET__301CFA4E2A70CD65");

            entity.ToTable("CHI_TIET_KM");

            entity.Property(e => e.MaKhuyenMai)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maKhuyenMai");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");
            entity.Property(e => e.PhanTramGiam).HasColumnName("phanTramGiam");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.ChiTietKms)
                .HasForeignKey(d => d.MaKhuyenMai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___maKhu__25518C17");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietKms)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET_K__maSP__2645B050");
        });

        modelBuilder.Entity<CuaHang>(entity =>
        {
            entity.HasKey(e => e.MaCuaHang).HasName("PK__CUA_HANG__325705DCCA2BC709");

            entity.ToTable("CUA_HANG");

            entity.Property(e => e.MaCuaHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maCuaHang");
            entity.Property(e => e.DiaChi).HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TenCuaHang)
                .HasMaxLength(100)
                .HasColumnName("tenCuaHang");
        });

        modelBuilder.Entity<DanhGiaSp>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DANH_GIA__6B15DD9A95611DCB");

            entity.ToTable("DANH_GIA_SP");

            entity.Property(e => e.MaDanhGia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maDanhGia");
            entity.Property(e => e.BinhLuan).HasColumnName("binhLuan");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");
            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maTaiKhoan");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayCapNhat");
            entity.Property(e => e.SoSao).HasColumnName("soSao");
            entity.Property(e => e.TrangThaiDg)
                .HasMaxLength(50)
                .HasColumnName("trangThaiDG");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.DanhGiaSps)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("FK__DANH_GIA_S__maSP__60A75C0F");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.DanhGiaSps)
                .HasForeignKey(d => d.MaTaiKhoan)
                .HasConstraintName("FK__DANH_GIA___maTai__619B8048");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DANH_MUC__6B0F914C76E52771");

            entity.ToTable("DANH_MUC");

            entity.Property(e => e.MaDanhMuc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maDanhMuc");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.TenDanhMuc)
                .HasMaxLength(100)
                .HasColumnName("tenDanhMuc");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DON_HANG__871D3819BAE39EF1");

            entity.ToTable("DON_HANG");

            entity.Property(e => e.MaDonHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maDonHang");
            entity.Property(e => e.DiaChiGh).HasColumnName("diaChiGH");
            entity.Property(e => e.MaPttt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maPTTT");
            entity.Property(e => e.MaPtvc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maPTVC");
            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maTaiKhoan");
            entity.Property(e => e.MaVoucher)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maVoucher");
            entity.Property(e => e.NgayDatHang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayDatHang");
            entity.Property(e => e.TongTienHang)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("tongTienHang");
            entity.Property(e => e.TrangThaiDonHang)
                .HasMaxLength(50)
                .HasColumnName("trangThaiDonHang");

            entity.HasOne(d => d.MaPtttNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaPttt)
                .HasConstraintName("FK__DON_HANG__maPTTT__6D0D32F4");

            entity.HasOne(d => d.MaPtvcNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaPtvc)
                .HasConstraintName("FK__DON_HANG__maPTVC__6FE99F9F");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DON_HANG__maTaiK__6EF57B66");

            entity.HasOne(d => d.MaVoucherNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaVoucher)
                .HasConstraintName("FK__DON_HANG__maVouc__6E01572D");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GIO_HANG__2C76D2030CFC4090");

            entity.ToTable("GIO_HANG");

            entity.HasIndex(e => e.MaTaiKhoan, "UQ__GIO_HANG__8FFF6A9CBF072ED8").IsUnique();

            entity.Property(e => e.MaGioHang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maGioHang");
            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maTaiKhoan");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithOne(p => p.GioHang)
                .HasForeignKey<GioHang>(d => d.MaTaiKhoan)
                .HasConstraintName("FK__GIO_HANG__maTaiK__656C112C");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KHUYEN_M__87BEDDE9C1A26EFA");

            entity.ToTable("KHUYEN_MAI");

            entity.Property(e => e.MaKhuyenMai)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maKhuyenMai");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.NgayBatDau)
                .HasColumnType("datetime")
                .HasColumnName("ngayBatDau");
            entity.Property(e => e.NgayKetThuc)
                .HasColumnType("datetime")
                .HasColumnName("ngayKetThuc");
            entity.Property(e => e.TenKhuyenMai)
                .HasMaxLength(100)
                .HasColumnName("tenKhuyenMai");
        });

        modelBuilder.Entity<PhuongThucTt>(entity =>
        {
            entity.HasKey(e => e.MaPttt).HasName("PK__PHUONG_T__90380CE2CA60FAF8");

            entity.ToTable("PHUONG_THUC_TT");

            entity.Property(e => e.MaPttt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maPTTT");
            entity.Property(e => e.TenPttt)
                .HasMaxLength(100)
                .HasColumnName("tenPTTT");
        });

        modelBuilder.Entity<PhuongThucVc>(entity =>
        {
            entity.HasKey(e => e.MaPtvc).HasName("PK__PHUONG_T__90385F92ACDF2DFE");

            entity.ToTable("PHUONG_THUC_VC");

            entity.Property(e => e.MaPtvc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maPTVC");
            entity.Property(e => e.PhiVanChuyen)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("phiVanChuyen");
            entity.Property(e => e.TenPtvc)
                .HasMaxLength(100)
                .HasColumnName("tenPTVC");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SAN_PHAM__7A227A7AE82A1B1B");

            entity.ToTable("SAN_PHAM");

            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maSP");
            entity.Property(e => e.DonVi)
                .HasMaxLength(50)
                .HasColumnName("donVi");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("gia");
            entity.Property(e => e.MaDanhMuc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maDanhMuc");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.SoLuongTon)
                .HasDefaultValue(0)
                .HasColumnName("soLuongTon");
            entity.Property(e => e.TenSp).HasMaxLength(100);
            entity.Property(e => e.TheTich)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("theTich");

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDanhMuc)
                .HasConstraintName("FK__SAN_PHAM__maDanh__5AEE82B9");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TAI_KHOA__8FFF6A9DFA9DCEA7");

            entity.ToTable("TAI_KHOAN");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TAI_KHOA__59267D4A64403C8C").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__TAI_KHOA__AB6E616424B5683B").IsUnique();

            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maTaiKhoan");
            entity.Property(e => e.DiaChi).HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.HoTen)
                .HasMaxLength(100)
                .HasColumnName("hoTen");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("matKhau");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tenDangNhap");
            entity.Property(e => e.VaiTro)
                .HasMaxLength(50)
                .HasColumnName("vaiTro");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.MaVoucher).HasName("PK__VOUCHER__E335C400CA0348A5");

            entity.ToTable("VOUCHER");

            entity.Property(e => e.MaVoucher)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maVoucher");
            entity.Property(e => e.DieuKienApDung)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("dieuKienApDung");
            entity.Property(e => e.GiamGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giamGia");
            entity.Property(e => e.NgayBatDau)
                .HasColumnType("datetime")
                .HasColumnName("ngayBatDau");
            entity.Property(e => e.NgayKetThuc)
                .HasColumnType("datetime")
                .HasColumnName("ngayKetThuc");
            entity.Property(e => e.TenVoucher)
                .HasMaxLength(100)
                .HasColumnName("tenVoucher");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
