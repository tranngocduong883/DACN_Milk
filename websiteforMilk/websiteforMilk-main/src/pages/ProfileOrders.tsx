import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { orderApi } from '../api'
import type { Order, OrderStatus } from '../api'
import { useApi } from '../hooks/useApi'
import { useStore } from '../context/StoreContext'
import { errorText, formatDate, formatPriceShort, orderStatusLabel, orderStatusStyle } from '../utils/format'
import { CalendarIcon, ClockIcon, RefreshIcon } from '../components/Icons'
import { ErrorBox, Loading } from '../components/Ui'

const filters: { value: OrderStatus | 'all'; label: string }[] = [
    { value: 'all', label: 'Tất cả' },
    { value: 'ChoXacNhan', label: orderStatusLabel.ChoXacNhan },
    { value: 'DangVanChuyen', label: orderStatusLabel.DangVanChuyen },
    { value: 'GiaoHangThanhCong', label: orderStatusLabel.GiaoHangThanhCong },
    { value: 'DaHuy', label: orderStatusLabel.DaHuy },
]

const ProfileOrders = () => {
    const { session, addToCart, notify } = useStore()
    const navigate = useNavigate()
    const userId = session?.userId ?? ''
    const { data, loading, error, reload } = useApi(() => orderApi.byUser(userId), [userId])
    const [filter, setFilter] = useState<OrderStatus | 'all'>('all')
    const [busy, setBusy] = useState('')

    const orders = (data ?? []).filter(o => filter === 'all' || o.status === filter)

    const cancel = async (o: Order) => {
        if (!confirm('Bạn chắc chắn muốn hủy đơn hàng này?')) return
        setBusy(o.id)
        try {
            notify(await orderApi.cancel(o.id, userId))
            await reload()
        } catch (e) {
            notify(errorText(e), 'error')
        } finally {
            setBusy('')
        }
    }

    const reorder = async (o: Order) => {
        setBusy(o.id)
        try {
            for (const i of o.items) await addToCart(i.productId, i.quantity)
            navigate('/cart')
        } catch {
            /* đã thông báo lỗi */
        } finally {
            setBusy('')
        }
    }

    return (
        <div className="space-y-1">
            <div className="rounded-t-lg bg-white p-7 pb-8">
                <div className="flex items-center justify-between rounded-lg p-6 shadow-sm ring-1 ring-gray-100">
                    <div>
                        <h1 className="text-2xl font-bold">Lịch sử mua hàng</h1>
                        <p className="mt-1 flex items-center gap-2 text-sm text-blue-600"><CalendarIcon size={16} /> Tất cả đơn hàng của bạn</p>
                    </div>
                    <div className="text-right">
                        <p className="text-sm text-gray-600">Tổng số đơn hàng</p>
                        <p className="text-2xl font-bold text-blue-600">{data?.length ?? 0} đơn</p>
                    </div>
                </div>
                <div className="mt-5 flex flex-wrap gap-2">
                    {filters.map(f => (
                        <button
                            key={f.value}
                            onClick={() => setFilter(f.value)}
                            className={`rounded-full border px-4 py-1.5 text-sm ${filter === f.value ? 'border-blue-600 bg-blue-600 text-white' : 'border-gray-300 hover:bg-gray-50'}`}
                        >
                            {f.label}
                        </button>
                    ))}
                </div>
            </div>

            {loading ? <Loading /> : error ? <ErrorBox message={error} onRetry={reload} /> : orders.length === 0 ? (
                <div className="rounded-lg bg-white p-10 text-center text-gray-500">
                    Bạn chưa có đơn hàng nào. <Link to="/" className="text-brand hover:underline">Mua sắm ngay</Link>
                </div>
            ) : orders.map(o => (
                <div key={o.id} className="rounded-lg bg-white text-sm shadow-sm">
                    <div className="space-y-2 border-b border-gray-200 p-5">
                        <div className="flex flex-wrap items-center gap-3">
                            <span className="rounded bg-blue-50 px-3 py-1.5 font-medium text-blue-600">Mã đơn: {o.id.slice(0, 8).toUpperCase()}</span>
                            <span className="flex items-center gap-1.5 text-gray-600"><CalendarIcon size={15} /> {formatDate(o.date)}</span>
                            <span className={`flex items-center gap-1.5 rounded-full border px-3 py-1 ${orderStatusStyle[o.status]}`}>
                                <ClockIcon size={15} /> {orderStatusLabel[o.status] ?? o.status}
                            </span>
                        </div>
                        <p><b className="font-medium">Địa chỉ:</b> {o.address}</p>
                        <p><b className="font-medium">PT thanh toán:</b> {o.payment}<br /><b className="font-medium">PT vận chuyển:</b> {o.shipping}</p>
                        <div className="pl-14 pt-2">
                            <p className="text-xl font-bold text-blue-600">{formatPriceShort(Math.max(0, o.total))}</p>
                            <p className="pl-4 text-gray-600">{o.items.length} sản phẩm</p>
                        </div>
                    </div>

                    <div className="p-5">
                        <h3 className="mb-4 text-lg font-medium">Chi tiết sản phẩm:</h3>
                        {o.items.map(i => (
                            <div key={i.productId} className="flex items-center justify-between border-b border-gray-100 py-4">
                                <div>
                                    <Link to={`/product/${i.productId}`} className="text-base hover:text-brand">{i.name}</Link>
                                    <p className="text-xs text-gray-500">SL: {i.quantity} × {formatPriceShort(i.price)}</p>
                                    <p className="text-xs text-gray-400">Mã: {i.productId}</p>
                                    {o.status === 'GiaoHangThanhCong' && (
                                        <Link to={`/product/${i.productId}`} className="text-xs text-blue-600 hover:underline">Đánh giá sản phẩm</Link>
                                    )}
                                </div>
                                <p className="text-lg font-medium">{formatPriceShort(i.price * i.quantity)}</p>
                            </div>
                        ))}

                        <div className="mt-6 space-y-2 border-t border-gray-200 pt-6 text-gray-600">
                            <div className="flex justify-between"><span>Tổng tiền hàng:</span><span>{formatPriceShort(o.subtotal)}</span></div>
                            <div className="flex justify-between"><span>Phí vận chuyển:</span><span>{formatPriceShort(o.shippingFee)}</span></div>
                            <div className="flex justify-between"><span>Giảm giá voucher:</span><span className="text-red-600">-{formatPriceShort(o.discount)}</span></div>
                        </div>
                        <div className="mt-3 flex justify-between border-y border-gray-200 py-5 text-lg">
                            <b>Thành tiền:</b><b className="text-blue-600">{formatPriceShort(Math.max(0, o.total))}</b>
                        </div>
                        <div className="mt-5 flex flex-col items-start gap-3">
                            {o.status === 'ChoXacNhan' && (
                                <button
                                    disabled={busy === o.id}
                                    onClick={() => cancel(o)}
                                    className="rounded-md border border-red-200 px-4 py-2 font-medium text-red-600 hover:bg-red-50 disabled:opacity-60"
                                >
                                    Hủy đơn hàng
                                </button>
                            )}
                            <button
                                disabled={busy === o.id}
                                onClick={() => reorder(o)}
                                className="flex items-center gap-2 rounded-md bg-blue-600 px-4 py-2 font-semibold text-white hover:bg-blue-700 disabled:opacity-60"
                            >
                                <RefreshIcon size={16} /> Đặt lại
                            </button>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    )
}

export default ProfileOrders
