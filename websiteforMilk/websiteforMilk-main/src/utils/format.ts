import type { OrderStatus, ReviewStatus } from '../api'

export const formatPrice = (n: number) => n.toLocaleString('vi-VN') + ' đ'
export const formatPriceShort = (n: number) => n.toLocaleString('vi-VN') + 'đ'

export const formatDate = (iso: string) => {
    const d = new Date(iso)
    return Number.isNaN(d.getTime()) ? iso : d.toLocaleDateString('vi-VN')
}

export const formatDateTime = (iso: string) => {
    const d = new Date(iso)
    return Number.isNaN(d.getTime()) ? iso : d.toLocaleString('vi-VN', { dateStyle: 'short', timeStyle: 'short' })
}

// yyyy-mm-dd cho <input type="date">
export const toDateInput = (iso: string) => (iso ? iso.slice(0, 10) : '')

export const orderStatusLabel: Record<OrderStatus, string> = {
    ChoXacNhan: 'Chờ xác nhận',
    DangVanChuyen: 'Đang giao',
    GiaoHangThanhCong: 'Giao hàng thành công',
    DaHuy: 'Đã hủy',
}

export const orderStatusStyle: Record<OrderStatus, string> = {
    ChoXacNhan: 'border-orange-300 bg-orange-50 text-orange-600',
    DangVanChuyen: 'border-blue-300 bg-blue-50 text-blue-600',
    GiaoHangThanhCong: 'border-green-300 bg-green-50 text-green-600',
    DaHuy: 'border-gray-300 bg-gray-50 text-gray-500',
}

// Luồng trạng thái hợp lệ theo DonHangService.UpdateTrangThaiDonHang
export const nextOrderStatuses: Record<OrderStatus, OrderStatus[]> = {
    ChoXacNhan: ['DangVanChuyen', 'DaHuy'],
    DangVanChuyen: ['GiaoHangThanhCong', 'DaHuy'],
    GiaoHangThanhCong: [],
    DaHuy: [],
}

export const reviewStatusLabel: Record<ReviewStatus, string> = {
    ChoDuyet: 'Chờ duyệt',
    DaDuyet: 'Đã duyệt',
    KhongHopLe: 'Không hợp lệ',
}

export const errorText = (e: unknown) => (e instanceof Error ? e.message : 'Đã có lỗi xảy ra')
