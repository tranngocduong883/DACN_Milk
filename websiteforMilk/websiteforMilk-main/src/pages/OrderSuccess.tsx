import { Link, useLocation } from 'react-router-dom'
import type { Order } from '../api'
import { formatDate, formatPriceShort } from '../utils/format'
import { CheckIcon } from '../components/Icons'

const OrderSuccess = () => {
    const order = (useLocation().state as { order?: Order } | null)?.order

    return (
        <div className="px-4 py-12">
            <div className="mx-auto max-w-[1080px] rounded-lg bg-white px-6 py-10 text-center shadow-md">
                <div className="mx-auto grid size-20 place-items-center rounded-full bg-green-100 text-green-600">
                    <CheckIcon size={34} strokeWidth={2.5} />
                </div>
                <h1 className="mt-6 text-3xl font-bold text-green-600">Yay, đặt hàng thành công!</h1>
                {order && (
                    <div className="mx-auto mt-6 max-w-md space-y-1 rounded-lg bg-gray-50 p-5 text-left text-gray-700">
                        <p><b>Mã đơn:</b> {order.id.slice(0, 8).toUpperCase()}</p>
                        <p><b>Phương thức thanh toán:</b> {order.payment}</p>
                        <p><b>Phương thức vận chuyển:</b> {order.shipping}</p>
                        <p><b>Dự kiến giao:</b> {formatDate(order.deliveryDate)}</p>
                        <p><b>Tổng thanh toán:</b> <span className="font-semibold text-blue-600">{formatPriceShort(Math.max(0, order.total))}</span></p>
                    </div>
                )}
                <div className="mt-8 flex flex-col items-center gap-5">
                    <Link to="/profile/orders" className="rounded-lg bg-blue-600 px-8 py-3.5 text-lg font-semibold text-white hover:bg-blue-700">
                        Theo dõi đơn hàng
                    </Link>
                    <Link to="/" className="rounded-lg bg-gray-100 px-8 py-3.5 text-lg font-semibold text-gray-700 hover:bg-gray-200">
                        Tiếp tục mua sắm
                    </Link>
                </div>
                <p className="mt-10 text-gray-600">Cảm ơn bạn đã mua hàng! Nếu có bất kỳ câu hỏi nào, hãy liên hệ với chúng tôi.</p>
                <p className="mt-3 text-gray-600">Hotline: 1800 545440 • Email: chamsockhachhang@thmilk.vn</p>
            </div>
        </div>
    )
}

export default OrderSuccess
