import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import { Navigate, useNavigate } from 'react-router-dom'
import { orderApi, PAYMENT_METHODS, SHIPPING_METHODS, shippingFeeFor, voucherApi } from '../api'
import type { Voucher } from '../api'
import { useApi } from '../hooks/useApi'
import { errorText, formatPriceShort } from '../utils/format'
import { useStore } from '../context/StoreContext'

const inputCls = 'w-full rounded border border-gray-300 px-3 py-2 outline-none focus:border-sky-dark'

const Checkout = () => {
    const { session, user, cart, cartTotal, reloadCart, notify } = useStore()
    const navigate = useNavigate()
    const userId = session?.userId ?? ''
    const { data: vouchers } = useApi(() => (userId ? voucherApi.availableFor(userId) : Promise.resolve([])), [userId])

    const [name, setName] = useState('')
    const [phone, setPhone] = useState('')
    const [address, setAddress] = useState('')
    const [note, setNote] = useState('')
    const [code, setCode] = useState('')
    const [voucher, setVoucher] = useState<Voucher | null>(null)
    const [voucherMsg, setVoucherMsg] = useState('')
    const [payment, setPayment] = useState(PAYMENT_METHODS[0].id)
    const [shipping, setShipping] = useState(SHIPPING_METHODS[1].id)
    const [submitting, setSubmitting] = useState(false)

    // Điền sẵn thông tin từ hồ sơ tài khoản
    useEffect(() => {
        if (!user) return
        setName(n => n || user.fullName)
        setPhone(p => p || user.phone)
        setAddress(a => a || user.address)
    }, [user])

    if (!session) return <Navigate to="/login" replace />
    if (cart.length === 0 && !submitting) return <Navigate to="/cart" replace />

    const shippingFee = shippingFeeFor(shipping, cartTotal)
    const discount = voucher && cartTotal >= voucher.minOrder ? voucher.discount : 0
    const total = Math.max(0, cartTotal + shippingFee - discount)

    const applyVoucher = async (raw = code) => {
        const c = raw.trim().toUpperCase()
        setCode(c)
        if (!c) { setVoucher(null); setVoucherMsg(''); return }
        // Ưu tiên danh sách voucher còn dùng được của tài khoản
        const v = vouchers?.find(x => x.code === c)
        if (!v) {
            try {
                await voucherApi.get(c)
                setVoucher(null)
                setVoucherMsg('Voucher đã hết hạn hoặc bạn đã sử dụng mã này')
            } catch {
                setVoucher(null)
                setVoucherMsg('Mã giảm giá không hợp lệ')
            }
            return
        }
        if (cartTotal < v.minOrder) {
            setVoucher(null)
            setVoucherMsg(`Đơn tối thiểu ${formatPriceShort(v.minOrder)} để dùng mã này`)
            return
        }
        setVoucher(v)
        setVoucherMsg(`Đã áp dụng: ${v.name}`)
    }

    const onSubmit = async (e: FormEvent) => {
        e.preventDefault()
        const store = shipping === 'PTVC01'
        const parts = [name.trim(), phone.trim(), store ? 'Nhận tại cửa hàng' : address.trim(), note.trim() && `Ghi chú: ${note.trim()}`]
        setSubmitting(true)
        try {
            const order = await orderApi.place({
                maTaiKhoan: userId,
                maPTTT: payment,
                maPTVC: shipping,
                maVoucher: voucher?.code,
                diaChiGiaoHang: parts.filter(Boolean).join(' - '),
            })
            await reloadCart()
            navigate('/order-success', { state: { order } })
        } catch (err) {
            notify(errorText(err), 'error')
            setSubmitting(false)
        }
    }

    return (
        <form onSubmit={onSubmit} className="grid gap-5 px-4 md:px-5 lg:grid-cols-[1fr_600px]">
            <div className="space-y-4 bg-white p-6 text-sm">
                <h2 className="text-lg font-medium uppercase">Địa chỉ giao hàng</h2>
                <label className="block">
                    <span className="mb-1 block font-semibold">Họ tên</span>
                    <input className={inputCls} value={name} onChange={e => setName(e.target.value)} required />
                </label>
                <label className="block">
                    <span className="mb-1 block font-semibold">Số điện thoại</span>
                    <input className={inputCls} value={phone} onChange={e => setPhone(e.target.value)} required pattern="0\d{9}" title="10 số, bắt đầu bằng 0" />
                </label>
                <label className="block">
                    <span className="mb-1 block font-semibold">Địa chỉ</span>
                    <textarea
                        className={`${inputCls} h-20`}
                        value={address}
                        onChange={e => setAddress(e.target.value)}
                        required={shipping !== 'PTVC01'}
                    />
                </label>
                {user?.address && (
                    <button type="button" onClick={() => setAddress(user.address)} className="rounded bg-sky-dark px-4 py-2 uppercase text-white hover:brightness-95">
                        Dùng địa chỉ mặc định
                    </button>
                )}
                <label className="block pt-6">
                    <span className="mb-1 block font-semibold">Ghi chú</span>
                    <input className={inputCls} value={note} onChange={e => setNote(e.target.value)} placeholder="Ghi chú cho đơn hàng" />
                </label>

                <h2 className="pt-8 text-lg font-medium uppercase">Phương thức vận chuyển</h2>
                <div className="space-y-3 border-t border-gray-800 pt-4 text-base">
                    {SHIPPING_METHODS.map(m => (
                        <label key={m.id} className="flex items-center gap-3">
                            <input type="radio" name="shipping" checked={shipping === m.id} onChange={() => setShipping(m.id)} className="accent-blue-600" />
                            {m.label}
                            <span className="text-gray-500">
                                ({m.fee === 0 ? 'Miễn phí' : `${formatPriceShort(m.fee)}, miễn phí cho đơn trên 300.000đ`})
                            </span>
                        </label>
                    ))}
                </div>

                <h2 className="pt-8 text-lg font-medium uppercase">Thông tin thanh toán</h2>
                <div className="space-y-3 border-t border-gray-800 pt-4 text-base">
                    {PAYMENT_METHODS.map(m => (
                        <label key={m.id} className="flex items-center gap-3">
                            <input type="radio" name="payment" checked={payment === m.id} onChange={() => setPayment(m.id)} className="accent-blue-600" />
                            {m.label}
                        </label>
                    ))}
                </div>
            </div>

            <aside className="self-start space-y-3 rounded-b-md bg-white p-6 text-sm shadow-sm">
                <h2 className="text-lg font-medium uppercase">Tóm tắt đơn hàng</h2>
                <div className="flex justify-between"><span>Tổng tiền</span><b>{formatPriceShort(cartTotal)}</b></div>
                <div className="flex justify-between border-b border-gray-800 pb-3"><span>Phí giao hàng</span><b>{formatPriceShort(shippingFee)}</b></div>
                {cart.map(({ product, quantity }) => (
                    <div key={product.id} className="flex justify-between gap-6">
                        <span>{product.name} x {quantity}</span>
                        <span>{formatPriceShort(product.price * quantity)}</span>
                    </div>
                ))}
                <div>
                    <p className="mb-2 font-semibold">Mã giảm giá</p>
                    <div className="flex gap-2">
                        <input value={code} onChange={e => setCode(e.target.value)} className={inputCls} placeholder="Nhập mã voucher" />
                        <button type="button" onClick={() => applyVoucher()} className="shrink-0 rounded bg-green-500 px-4 text-base text-white hover:bg-green-600">Áp dụng</button>
                    </div>
                    {voucherMsg && <p className={`mt-1 ${voucher ? 'text-green-600' : 'text-red-600'}`}>{voucherMsg}</p>}
                    {vouchers && vouchers.length > 0 && (
                        <div className="mt-2 flex flex-wrap gap-2">
                            {vouchers.map(v => (
                                <button
                                    type="button"
                                    key={v.code}
                                    onClick={() => applyVoucher(v.code)}
                                    title={v.description}
                                    disabled={cartTotal < v.minOrder}
                                    className="rounded border border-dashed border-green-500 px-2 py-1 text-xs text-green-700 hover:bg-green-50 disabled:border-gray-300 disabled:text-gray-400"
                                >
                                    {v.code} · -{formatPriceShort(v.discount)}
                                </button>
                            ))}
                        </div>
                    )}
                </div>
                {discount > 0 && (
                    <div className="flex justify-between text-red-600"><span>Giảm giá voucher</span><span>-{formatPriceShort(discount)}</span></div>
                )}
                <div className="flex justify-between border-t border-gray-800 pt-4 text-lg font-medium">
                    <span>Tổng thanh toán</span><span>{formatPriceShort(total)}</span>
                </div>
                <button disabled={submitting} className="w-full rounded bg-sky-dark py-3 text-base font-semibold uppercase text-white hover:brightness-95 disabled:opacity-60">
                    {submitting ? 'Đang đặt hàng...' : 'Thanh toán'}
                </button>
            </aside>
        </form>
    )
}

export default Checkout
