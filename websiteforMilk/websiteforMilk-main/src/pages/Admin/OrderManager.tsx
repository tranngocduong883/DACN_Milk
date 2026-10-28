import { useState } from 'react'
import { orderApi } from '../../api'
import type { Order, OrderStatus } from '../../api'
import { useApi } from '../../hooks/useApi'
import { useStore } from '../../context/StoreContext'
import { errorText, formatDateTime, formatPriceShort, nextOrderStatuses, orderStatusLabel, orderStatusStyle } from '../../utils/format'
import { ErrorBox, Loading, Modal } from '../../components/Ui'

const OrderManager = () => {
    const { notify } = useStore()
    const { data, loading, error, reload } = useApi(() => orderApi.all(), [])
    const [detail, setDetail] = useState<Order | null>(null)
    const [filter, setFilter] = useState<OrderStatus | 'all'>('all')

    const act = async (fn: () => Promise<string>) => {
        try {
            notify(await fn())
            const list = await orderApi.all()
            setDetail(d => (d ? list.find(o => o.id === d.id) ?? null : null))
            await reload()
        } catch (e) {
            notify(errorText(e), 'error')
        }
    }

    const changeStatus = (o: Order, status: OrderStatus) => {
        if (confirm(`Chuyển đơn ${o.id.slice(0, 8).toUpperCase()} sang "${orderStatusLabel[status]}"?`)) {
            act(() => orderApi.updateStatus(o.id, status))
        }
    }

    const remove = (o: Order) => {
        if (confirm('Xóa đơn hàng này?')) act(() => orderApi.remove(o.id))
    }

    if (loading && !data) return <Loading />
    if (error && !data) return <ErrorBox message={error} onRetry={reload} />

    const list = (data ?? []).filter(o => filter === 'all' || o.status === filter)

    return (
        <div>
            <div className="mb-6 flex flex-wrap items-center justify-between gap-4">
                <h1 className="text-3xl font-bold">Quản lý đơn hàng</h1>
                <select
                    value={filter}
                    onChange={e => setFilter(e.target.value as OrderStatus | 'all')}
                    className="rounded border border-gray-300 px-3 py-2"
                >
                    <option value="all">Tất cả trạng thái</option>
                    {(Object.keys(orderStatusLabel) as OrderStatus[]).map(s => <option key={s} value={s}>{orderStatusLabel[s]}</option>)}
                </select>
            </div>
            {list.length === 0 ? (
                <p className="rounded bg-white p-10 text-center text-gray-500">Chưa có đơn hàng nào.</p>
            ) : (
                <div className="overflow-x-auto">
                    <table className="admin-table text-lg">
                        <thead>
                            <tr><th>STT</th><th>MÃ ĐƠN</th><th>KHÁCH HÀNG</th><th>NGÀY ĐẶT</th><th>TRẠNG THÁI</th><th>TỔNG TIỀN</th><th>THANH TOÁN</th><th>HÀNH ĐỘNG</th></tr>
                        </thead>
                        <tbody>
                            {list.map((o, i) => (
                                <tr key={o.id}>
                                    <td>{i + 1}</td>
                                    <td className="font-mono text-base">{o.id.slice(0, 8).toUpperCase()}</td>
                                    <td>{o.customer}</td>
                                    <td>{formatDateTime(o.date)}</td>
                                    <td>
                                        <span className={`whitespace-nowrap rounded border px-2 py-0.5 text-sm ${orderStatusStyle[o.status]}`}>
                                            {orderStatusLabel[o.status] ?? o.status}
                                        </span>
                                    </td>
                                    <td className="font-semibold text-green-600">{formatPriceShort(Math.max(0, o.total))}</td>
                                    <td className="text-base">{o.payment}</td>
                                    <td className="whitespace-nowrap">
                                        <button onClick={() => setDetail(o)} className="mr-2 rounded bg-blue-600 px-2 py-0.5 text-white hover:bg-blue-700">Chi tiết</button>
                                        <button
                                            disabled={o.status !== 'DaHuy'}
                                            title={o.status !== 'DaHuy' ? 'Chỉ xóa được đơn đã hủy' : undefined}
                                            onClick={() => remove(o)}
                                            className="rounded border border-red-400 px-2 py-0.5 text-red-500 hover:bg-red-50 disabled:border-gray-200 disabled:text-gray-300"
                                        >
                                            Xóa
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}

            {detail && (
                <Modal title={`Đơn hàng ${detail.id.slice(0, 8).toUpperCase()}`} onClose={() => setDetail(null)} wide>
                    <div className="space-y-2 text-gray-700">
                        <p><b>Mã đầy đủ:</b> <span className="font-mono text-sm">{detail.id}</span></p>
                        <p><b>Khách hàng:</b> {detail.customer}</p>
                        <p><b>Ngày đặt:</b> {formatDateTime(detail.date)}</p>
                        <p><b>Địa chỉ giao:</b> {detail.address}</p>
                        <p><b>Thanh toán:</b> {detail.payment} · <b>Vận chuyển:</b> {detail.shipping}</p>
                        <p>
                            <b>Trạng thái:</b>{' '}
                            <span className={`rounded border px-2 py-0.5 text-sm ${orderStatusStyle[detail.status]}`}>{orderStatusLabel[detail.status]}</span>
                        </p>
                    </div>
                    <table className="admin-table mt-5">
                        <thead><tr><th>Sản phẩm</th><th>SL</th><th>Đơn giá</th><th>Thành tiền</th></tr></thead>
                        <tbody>
                            {detail.items.map(i => (
                                <tr key={i.productId}>
                                    <td className="!text-left">{i.name}</td>
                                    <td>{i.quantity}</td>
                                    <td>{formatPriceShort(i.price)}</td>
                                    <td>{formatPriceShort(i.price * i.quantity)}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    <div className="mt-4 space-y-1 text-right">
                        <p>Tiền hàng: {formatPriceShort(detail.subtotal)}</p>
                        <p>Phí ship: {formatPriceShort(detail.shippingFee)}</p>
                        <p>Giảm giá: -{formatPriceShort(detail.discount)}</p>
                        <p className="text-lg font-bold text-blue-600">Thành tiền: {formatPriceShort(Math.max(0, detail.total))}</p>
                    </div>
                    {nextOrderStatuses[detail.status]?.length > 0 && (
                        <div className="mt-6 flex flex-wrap justify-end gap-3 border-t border-gray-200 pt-4">
                            {nextOrderStatuses[detail.status].map(s => (
                                <button
                                    key={s}
                                    onClick={() => changeStatus(detail, s)}
                                    className={`rounded px-4 py-2 text-white ${s === 'DaHuy' ? 'bg-red-500 hover:bg-red-600' : 'bg-green-600 hover:bg-green-700'}`}
                                >
                                    {s === 'DaHuy' ? 'Hủy đơn' : `Chuyển sang: ${orderStatusLabel[s]}`}
                                </button>
                            ))}
                        </div>
                    )}
                </Modal>
            )}
        </div>
    )
}

export default OrderManager
