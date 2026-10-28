import { Link, Navigate } from 'react-router-dom'
import { formatPrice } from '../utils/format'
import { useStore } from '../context/StoreContext'
import { TrashIcon } from '../components/Icons'

const Cart = () => {
    const { session, cart, cartTotal, updateQuantity, removeFromCart } = useStore()

    if (!session) return <Navigate to="/login" replace />

    // Lỗi đã được thông báo bằng toast trong StoreContext
    const safe = (p: Promise<void>) => { p.catch(() => {}) }

    return (
        <div className="px-4 md:px-10">
            <div className="rounded-b-lg bg-white px-6 py-10 shadow-sm md:px-10">
                <h1 className="mb-10 text-center text-4xl font-bold">GIỎ HÀNG CỦA BẠN</h1>

                {cart.length === 0 ? (
                    <div className="py-10 text-center text-gray-500">
                        Giỏ hàng của bạn đang trống.{' '}
                        <Link to="/" className="text-brand hover:underline">Tiếp tục mua sắm</Link>
                    </div>
                ) : (
                    <>
                        <div className="overflow-x-auto">
                            <table className="w-full min-w-[720px]">
                                <thead>
                                    <tr className="border-b border-gray-800 text-lg uppercase">
                                        <th className="py-5 text-left font-medium">Sản phẩm</th>
                                        <th className="font-medium">Giá</th>
                                        <th className="font-medium">Số lượng</th>
                                        <th className="font-medium">Thành tiền</th>
                                        <th className="font-medium">Xóa</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {cart.map(({ product, quantity }) => (
                                        <tr key={product.id} className="border-b border-gray-800 text-center text-lg">
                                            <td className="py-10">
                                                <Link to={`/product/${product.id}`} className="flex items-center gap-5 text-left font-medium hover:text-brand">
                                                    <img src={product.image} alt="" className="size-24 shrink-0 rounded object-contain" />
                                                    {product.name}
                                                </Link>
                                            </td>
                                            <td>
                                                {formatPrice(product.price)}
                                                {product.price < product.originalPrice && <span className="block text-sm text-gray-400 line-through">{formatPrice(product.originalPrice)}</span>}
                                            </td>
                                            <td>
                                                <div className="inline-flex items-center gap-8">
                                                    <button disabled={quantity <= 1} onClick={() => safe(updateQuantity(product.id, quantity - 1))} className="size-10 rounded bg-gray-200 hover:bg-gray-300 disabled:opacity-40">-</button>
                                                    <span>{quantity}</span>
                                                    <button onClick={() => safe(updateQuantity(product.id, quantity + 1))} className="size-10 rounded bg-gray-200 hover:bg-gray-300">+</button>
                                                </div>
                                            </td>
                                            <td className="text-xl text-blue-600">{formatPrice(product.price * quantity)}</td>
                                            <td>
                                                <button onClick={() => safe(removeFromCart(product.id))} className="text-red-500 hover:text-red-700" aria-label="Xóa">
                                                    <TrashIcon size={24} />
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>

                        <div className="mt-16 flex flex-wrap items-end justify-between gap-6">
                            <p className="text-lg text-gray-600">
                                Bạn có thể <Link to="/" className="hover:text-brand">tiếp tục mua sắm</Link> hoặc thanh toán
                            </p>
                            <div className="text-right">
                                <p className="text-2xl font-medium">Tổng cộng:</p>
                                <p className="text-3xl font-bold text-blue-600">{formatPrice(cartTotal)}</p>
                                <Link to="/checkout" className="mt-4 inline-block rounded-md bg-sky px-10 py-3 font-semibold uppercase text-white hover:bg-sky-dark">
                                    Thanh toán
                                </Link>
                            </div>
                        </div>
                    </>
                )}
            </div>
        </div>
    )
}

export default Cart
