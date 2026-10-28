import { getData, getMessage } from './client'

// ===== Kiểu dữ liệu backend trả về (DACN_H_P.Dtos.Response) =====

interface DanhMucDto { maDanhMuc: string; tenDanhMuc: string; moTa?: string | null }
interface SanPhamDto {
    maSp: string; tenSp: string; maDanhMuc?: string | null; tenDanhMuc?: string | null
    gia: number; giaKhuyenMai: number; moTa?: string | null; soLuongTon?: number | null
    theTich?: number | null; donVi?: string | null; anhDaiDien?: string | null; duongDanAnhSPs: string[]
}
interface TaiKhoanDto {
    maTaiKhoan: string; tenDangNhap: string; email?: string | null; hoTen?: string | null
    soDienThoai?: string | null; vaiTro?: string | null; diaChi?: string | null
}
interface DangNhapDto { maTaiKhoan: string; tenDangNhap: string; email?: string | null; vaiTro?: string | null; token: string }
interface ChiTietGHDto { maSp: string; tenSp: string; anh?: string | null; soLuong: number; giaBanDau: number; giaDatHang: number }
interface GioHangDto { maGioHang: string; sanPhams: ChiTietGHDto[]; tongTien: number }
interface ChiTietDonHangDto { maSP: string; tenSP: string; soLuong: number; donGia: number }
interface DonHangDto {
    maDonHang: string; maTaiKhoan: string; tenNguoiDat?: string | null; ngayDatHang: string; trangThaiDonHang: string
    tongTienHang: number; diaChiGiaoHang: string; phuongThucThanhToan: string; phuongThucVanChuyen: string
    phiShip?: number | null; giamGiaVoucher?: number | null; ngayGiaoHang: string; thanhToan: number
    chiTietDonHang: ChiTietDonHangDto[]
}
interface VoucherDto {
    maVoucher: string; tenVoucher: string; giamGia: number; dieuKienApDung: number; moTa?: string | null
    ngayBatDau: string; ngayKetThuc: string; trangThai: string
}
interface DanhGiaDto {
    maDanhGia: string; maSp: string; tenSp: string; maTaiKhoan: string; tenNguoiDung: string
    soSao: number; binhLuan?: string | null; ngayCapNhat: string; trangThai: string
}
interface CuaHangDto { maCuaHang: string; tenCuaHang: string; diaChi?: string | null; soDienThoai?: string | null; email?: string | null; moTa?: string | null }

// ===== Kiểu dữ liệu dùng trong giao diện =====

export interface Category { id: string; name: string; description: string }

export interface Product {
    id: string
    name: string
    price: number // giá bán thực tế (đã áp khuyến mãi)
    originalPrice: number
    unit: string
    categoryId: string
    categoryName: string
    image: string
    gallery: string[]
    description: string
    stock: number
    volume: number | null
}

export type Role = 'admin' | 'user'

export interface User {
    id: string
    username: string
    fullName: string
    phone: string
    email: string
    address: string
    role: Role
}

export interface CartItem {
    product: Pick<Product, 'id' | 'name' | 'image' | 'price' | 'originalPrice'>
    quantity: number
}

export type OrderStatus = 'ChoXacNhan' | 'DangVanChuyen' | 'GiaoHangThanhCong' | 'DaHuy'

export interface OrderItem { productId: string; name: string; price: number; quantity: number }

export interface Order {
    id: string
    userId: string
    customer: string
    date: string
    deliveryDate: string
    status: OrderStatus
    address: string
    payment: string
    shipping: string
    items: OrderItem[]
    subtotal: number
    shippingFee: number
    discount: number
    total: number
}

export interface Voucher {
    code: string
    name: string
    discount: number
    minOrder: number
    description: string
    startDate: string
    endDate: string
    status: string
}

export type ReviewStatus = 'ChoDuyet' | 'DaDuyet' | 'KhongHopLe'

export interface Review {
    id: string
    productId: string
    productName: string
    userId: string
    userName: string
    stars: number
    comment: string
    date: string
    status: ReviewStatus
}

export interface Store { id: string; name: string; address: string; phone: string; email: string; description: string }

// ===== Chuyển đổi DTO -> model =====

const FALLBACK_IMAGE = '/images/p1.png'

const toCategory = (d: DanhMucDto): Category => ({ id: d.maDanhMuc, name: d.tenDanhMuc, description: d.moTa ?? '' })

const toProduct = (d: SanPhamDto): Product => {
    const gallery = d.duongDanAnhSPs?.length ? d.duongDanAnhSPs : [FALLBACK_IMAGE]
    return {
        id: d.maSp,
        name: d.tenSp,
        price: d.giaKhuyenMai || d.gia,
        originalPrice: d.gia,
        unit: d.donVi ?? '',
        categoryId: d.maDanhMuc ?? '',
        categoryName: d.tenDanhMuc ?? '',
        image: d.anhDaiDien ?? gallery[0],
        gallery,
        description: d.moTa ?? '',
        stock: d.soLuongTon ?? 0,
        volume: d.theTich ?? null,
    }
}

const toUser = (d: TaiKhoanDto): User => ({
    id: d.maTaiKhoan,
    username: d.tenDangNhap,
    fullName: d.hoTen ?? '',
    phone: d.soDienThoai ?? '',
    email: d.email ?? '',
    address: d.diaChi ?? '',
    role: d.vaiTro?.toLowerCase() === 'admin' ? 'admin' : 'user',
})

const toCart = (d: GioHangDto | null): CartItem[] =>
    (d?.sanPhams ?? []).map(i => ({
        product: { id: i.maSp, name: i.tenSp, image: i.anh ?? FALLBACK_IMAGE, price: i.giaDatHang, originalPrice: i.giaBanDau },
        quantity: i.soLuong,
    }))

const toOrder = (d: DonHangDto): Order => ({
    id: d.maDonHang,
    userId: d.maTaiKhoan,
    customer: d.tenNguoiDat ?? '',
    date: d.ngayDatHang,
    deliveryDate: d.ngayGiaoHang,
    status: d.trangThaiDonHang as OrderStatus,
    address: d.diaChiGiaoHang,
    payment: d.phuongThucThanhToan,
    shipping: d.phuongThucVanChuyen,
    items: d.chiTietDonHang.map(i => ({ productId: i.maSP, name: i.tenSP, price: i.donGia, quantity: i.soLuong })),
    subtotal: d.tongTienHang,
    shippingFee: d.phiShip ?? 0,
    discount: d.giamGiaVoucher ?? 0,
    total: d.thanhToan,
})

const toVoucher = (d: VoucherDto): Voucher => ({
    code: d.maVoucher,
    name: d.tenVoucher ?? '',
    discount: d.giamGia,
    minOrder: d.dieuKienApDung ?? 0,
    description: d.moTa ?? '',
    startDate: d.ngayBatDau,
    endDate: d.ngayKetThuc,
    status: d.trangThai,
})

const toReview = (d: DanhGiaDto): Review => ({
    id: d.maDanhGia,
    productId: d.maSp,
    productName: d.tenSp,
    userId: d.maTaiKhoan,
    userName: d.tenNguoiDung,
    stars: d.soSao,
    comment: d.binhLuan ?? '',
    date: d.ngayCapNhat,
    status: d.trangThai as ReviewStatus,
})

const toStore = (d: CuaHangDto): Store => ({
    id: d.maCuaHang,
    name: d.tenCuaHang,
    address: d.diaChi ?? '',
    phone: d.soDienThoai ?? '',
    email: d.email ?? '',
    description: d.moTa ?? '',
})

const enc = encodeURIComponent

// ===== Endpoint =====

export const authApi = {
    login: (username: string, password: string) =>
        getData<DangNhapDto>('POST', '/api/Authentication/Login', { tenDangNhap: username, matKhau: password }),
    register: (body: { tenDangNhap: string; matKhau: string; hoTen?: string; email?: string; soDienThoai?: string; diaChi?: string }) =>
        getMessage('POST', '/api/Authentication/Register', body),
}

export const userApi = {
    get: async (id: string) => toUser(await getData<TaiKhoanDto>('GET', `/api/TaiKhoan/${enc(id)}`)),
    all: async () => (await getData<TaiKhoanDto[]>('GET', '/api/TaiKhoan/admin/all')).map(toUser),
    updateProfile: async (id: string, body: { hoTen?: string; soDienThoai?: string; diaChi?: string; email?: string }) =>
        toUser(await getData<TaiKhoanDto>('PUT', `/api/TaiKhoan/profile/${enc(id)}`, body)),
    changePassword: (id: string, oldPassword: string, newPassword: string) =>
        getMessage('PUT', `/api/TaiKhoan/change-password/${enc(id)}`, { matKhauCu: oldPassword, matKhauMoi: newPassword }),
    updateRole: (id: string, role: Role) => getMessage('PUT', `/api/TaiKhoan/admin/role/${enc(id)}`, { vaiTroMoi: role }),
    remove: (id: string) => getMessage('DELETE', `/api/TaiKhoan/admin/${enc(id)}`),
}

export interface CategoryInput { maDanhMuc: string; tenDanhMuc: string; moTa?: string }

export const categoryApi = {
    all: async () => (await getData<DanhMucDto[]>('GET', '/api/DanhMuc')).map(toCategory),
    create: (body: CategoryInput) => getMessage('POST', '/api/DanhMuc', body),
    update: (body: CategoryInput) => getMessage('PUT', '/api/DanhMuc', body),
    remove: (id: string) => getMessage('DELETE', `/api/DanhMuc?madm=${enc(id)}`),
}

export interface ProductInput {
    maSp: string; tenSp: string; maDanhMuc: string; gia: number; moTa?: string
    soLuongTon?: number; theTich?: number; donVi?: string; files?: File[]
}

const productForm = (p: ProductInput) => {
    const fd = new FormData()
    fd.append('MaSp', p.maSp)
    fd.append('TenSp', p.tenSp)
    fd.append('MaDanhMuc', p.maDanhMuc)
    fd.append('Gia', String(p.gia))
    if (p.moTa) fd.append('MoTa', p.moTa)
    if (p.soLuongTon != null) fd.append('SoLuongTon', String(p.soLuongTon))
    if (p.theTich != null) fd.append('TheTich', String(p.theTich))
    if (p.donVi) fd.append('DonVi', p.donVi)
    p.files?.forEach(f => fd.append('AnhSps', f))
    return fd
}

export const productApi = {
    all: async () => (await getData<SanPhamDto[]>('GET', '/api/SanPham')).map(toProduct),
    get: async (id: string) => toProduct(await getData<SanPhamDto>('GET', `/api/SanPham/by-masp/${enc(id)}`)),
    create: (p: ProductInput) => getMessage('POST', '/api/SanPham', productForm(p)),
    update: (p: ProductInput) => getMessage('PUT', '/api/SanPham', productForm(p)),
    remove: (id: string) => getMessage('DELETE', `/api/SanPham?masp=${enc(id)}`),
}

export const cartApi = {
    get: async (userId: string) => toCart(await getData<GioHangDto>('GET', `/api/GioHang?matk=${enc(userId)}`)),
    add: async (userId: string, productId: string, quantity: number) =>
        toCart(await getData<GioHangDto>('POST', `/api/GioHang/addOrupdate/${enc(userId)}`, { maSP: productId, soLuong: quantity })),
    setQuantity: async (userId: string, productId: string, quantity: number) =>
        toCart(await getData<GioHangDto>('POST', `/api/GioHang/updateItem/${enc(userId)}`, { maSP: productId, soLuong: quantity })),
    remove: async (userId: string, productId: string) =>
        toCart(await getData<GioHangDto>('DELETE', `/api/GioHang/removeItem/${enc(userId)}/${enc(productId)}`)),
    clear: async (userId: string) => toCart(await getData<GioHangDto>('DELETE', `/api/GioHang/removeAll/${enc(userId)}`)),
}

export interface PlaceOrderInput { maTaiKhoan: string; maPTTT: string; maPTVC: string; maVoucher?: string; diaChiGiaoHang: string }

export const orderApi = {
    all: async () => (await getData<DonHangDto[]>('GET', '/api/DonHang')).map(toOrder),
    byUser: async (userId: string) => (await getData<DonHangDto[]>('GET', `/api/DonHang/user/${enc(userId)}`)).map(toOrder),
    get: async (id: string) => toOrder(await getData<DonHangDto>('GET', `/api/DonHang/${enc(id)}`)),
    place: async (body: PlaceOrderInput) => toOrder(await getData<DonHangDto>('POST', '/api/DonHang', body)),
    cancel: (id: string, userId: string) => getMessage('PUT', `/api/DonHang/huydon/${enc(id)}`, { maTaiKhoan: userId }),
    updateStatus: (id: string, status: OrderStatus) => getMessage('PUT', `/api/DonHang/trangthai/${enc(id)}`, { trangThaiMoi: status }),
    remove: (id: string) => getMessage('DELETE', `/api/DonHang?maDH=${enc(id)}`),
}

export interface VoucherInput {
    maVoucher: string; tenVoucher: string; giamGia: number; dieuKienApDung: number
    ngayBatDau: string; ngayKetThuc: string; moTa?: string
}

export const voucherApi = {
    all: async () => (await getData<VoucherDto[]>('GET', '/api/Voucher')).map(toVoucher),
    get: async (code: string) => toVoucher(await getData<VoucherDto>('GET', `/api/Voucher/${enc(code)}`)),
    availableFor: async (userId: string) => (await getData<VoucherDto[]>('GET', `/api/Voucher/available/${enc(userId)}`)).map(toVoucher),
    create: (body: VoucherInput) => getMessage('POST', '/api/Voucher', body),
    update: (body: VoucherInput) => getMessage('PUT', '/api/Voucher', body),
    remove: (code: string) => getMessage('DELETE', `/api/Voucher?maVoucher=${enc(code)}`),
}

export const reviewApi = {
    byProduct: async (productId: string) => (await getData<DanhGiaDto[]>('GET', `/api/DanhGia/product/${enc(productId)}`)).map(toReview),
    all: async () => (await getData<DanhGiaDto[]>('GET', '/api/DanhGia/admin/list')).map(toReview),
    create: (body: { maSp: string; maTaiKhoan: string; soSao: number; binhLuan?: string }) =>
        getMessage('POST', '/api/DanhGia/create', body),
    setStatus: (id: string, status: Exclude<ReviewStatus, 'ChoDuyet'>) =>
        getMessage('PUT', `/api/DanhGia/admin/approve/${enc(id)}`, { trangThai: status }),
    remove: (id: string) => getMessage('DELETE', `/api/DanhGia/admin/delete/${enc(id)}`),
}

export const storeApi = {
    all: async () => (await getData<CuaHangDto[]>('GET', '/api/CuaHang')).map(toStore),
}

// Phương thức thanh toán / vận chuyển (bảng PHUONG_THUC_TT, PHUONG_THUC_VC trong init-db.sql)
export const PAYMENT_METHODS = [
    { id: 'PTTT01', label: 'Thanh toán khi nhận hàng (COD)' },
    { id: 'PTTT02', label: 'Thanh toán qua VNPAY' },
]
export const SHIPPING_METHODS = [
    { id: 'PTVC01', label: 'Nhận tại cửa hàng', fee: 0 },
    { id: 'PTVC02', label: 'Giao hàng tại nhà', fee: 30000 },
]
// Giống DonHangBuilder ở backend: giao tại nhà miễn phí ship khi tiền hàng > 300.000đ
export const shippingFeeFor = (methodId: string, subtotal: number) => {
    const m = SHIPPING_METHODS.find(s => s.id === methodId)
    if (!m || m.fee === 0) return 0
    return subtotal > 300000 ? 0 : m.fee
}
